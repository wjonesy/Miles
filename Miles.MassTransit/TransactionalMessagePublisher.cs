/*
 *     Copyright 2016 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using MassTransit;
using Miles.Messaging;
using Miles.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Dispatches events and commands on transaction commit. Stores messages and events
    /// within a data store, subject to the transaction, with consistant message Ids to aid
    /// in message de-duplication.
    /// </summary>
    /// <seealso cref="Miles.Events.IEventPublisher" />
    /// <seealso cref="Miles.Events.ICommandPublisher" />
    public class TransactionalMessagePublisher : IEventPublisher, ICommandPublisher
    {
        private readonly IOutgoingMessageRepository outgoingEventRepository;
        private readonly ITime time;

        // State
        private readonly Stack<PendingMessageHandler> messageStack = new Stack<PendingMessageHandler>();
        private readonly List<OutgoingMessageAndObject> pendingDispatchMessages = new List<OutgoingMessageAndObject>();
        private readonly IActivityContext activityContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionalMessagePublisher" /> class.
        /// </summary>
        /// <param name="transactionContext">The transaction context.</param>
        /// <param name="outgoingEventRepository">The outgoing event repository.</param>
        /// <param name="time">The time.</param>
        /// <param name="activityContext">The activity context.</param>
        /// <param name="commandDispatcher">The command dispatcher.</param>
        /// <param name="eventDispatcher">The event dispatcher.</param>
        public TransactionalMessagePublisher(
            ITransactionContext transactionContext,
            IOutgoingMessageRepository outgoingEventRepository,
            ITime time,
            IActivityContext activityContext,
            IMessageDispatcher commandDispatcher,
            ConventionBasedMessageDispatcher eventDispatcher)
        {
            this.messageStack.Push(new PendingMessageHandler(this));
            this.outgoingEventRepository = outgoingEventRepository;
            this.time = time;
            this.activityContext = activityContext;

            transactionContext.PreCommit.Register((s, e) => messageStack.Peek().Handle());

            transactionContext.PostCommit.Register(async (s, e) =>
            {
                foreach (var message in pendingDispatchMessages)
                {
                    // After commit try to dispatch the messages. Try to mark them as dispatched.
                    try
                    {
                        var dispatcher = message.OutgoingMessage.MessageType == OutgoingMessageType.Command ? commandDispatcher : eventDispatcher;
                        await dispatcher.DispatchAsync(message.MessageObject, message.OutgoingMessage).ConfigureAwait(false);
                        message.OutgoingMessage.Dispatched(time.Now);
                        await outgoingEventRepository.SaveAsync(message.OutgoingMessage, ignoreTransaction: true).ConfigureAwait(false);
                    }
                    catch
                    {
                        // TODO: Report the failure, but we are intentionally hiding this problem
                    }
                }
                pendingDispatchMessages.Clear();
            });
        }

        #region IEventPublisher

        void IEventPublisher.Register<TEvent>(IMessageProcessor<TEvent> evt)
        {
            messageStack.Peek().Register(evt);
        }

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="evt">The event.</param>
        void IEventPublisher.Publish<TEvent>(TEvent evt)
        {
            messageStack.Peek().Publish(OutgoingMessageType.Event, evt);
        }

        #endregion

        #region ICommandPublisher

        void ICommandPublisher.Register<TCommand>(IMessageProcessor<TCommand> cmd)
        {
            messageStack.Peek().Register(cmd);
        }

        /// <summary>
        /// Publishes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="cmd">The command.</param>
        void ICommandPublisher.Publish<TCommand>(TCommand cmd)
        {
            messageStack.Peek().Publish(OutgoingMessageType.Command, cmd);
        }

        #endregion

        #region Register and calling message handlers immediately

        // Internal structure to keep the message object and its db representation together
        private class OutgoingMessageAndObject
        {
            public OutgoingMessageAndObject(OutgoingMessageType outgoingMessageType, Type messageType, Object messageObject)
            {
                this.MessageType = messageType;
                this.MessageObject = messageObject;
                this.OutgoingMessageType = outgoingMessageType;
            }

            public OutgoingMessageType OutgoingMessageType { get; private set; }
            public Type MessageType { get; private set; }
            public Object MessageObject { get; private set; }

            public OutgoingMessage OutgoingMessage { get; private set; }

            public OutgoingMessage GenerateOutgoingMessage(Guid correlationId, DateTime eventCreated)
            {
                if (OutgoingMessage == null)
                    OutgoingMessage = new OutgoingMessage(NewId.NextGuid(), correlationId, MessageType.FullName, OutgoingMessageType, JsonConvert.SerializeObject(MessageObject), eventCreated);

                return OutgoingMessage;
            }
        }

        private class PendingMessageHandler
        {
            private readonly TransactionalMessagePublisher publisher;
            private readonly List<OutgoingMessageAndObject> pendingSaveMessages = new List<OutgoingMessageAndObject>();
            private readonly Dictionary<Type, List<IObjectMessageProcessor>> immediateMessageHandlers = new Dictionary<Type, List<IObjectMessageProcessor>>();

            public PendingMessageHandler(TransactionalMessagePublisher publisher)
            {
                this.publisher = publisher;
            }

            public void Register<TMessage>(IMessageProcessor<TMessage> messageHandler) where TMessage : class
            {
                var messageType = typeof(TMessage);

                List<IObjectMessageProcessor> handlers;
                if (!immediateMessageHandlers.TryGetValue(messageType, out handlers))
                {
                    handlers = new List<IObjectMessageProcessor>();
                    immediateMessageHandlers.Add(messageType, handlers);
                }

                handlers.Add(new ObjectToGenericMessageProcessor<TMessage>(messageHandler));
            }

            public void Publish<TMessage>(OutgoingMessageType type, TMessage msg)
            {
                pendingSaveMessages.Add(new OutgoingMessageAndObject(type, typeof(TMessage), msg));
            }

            public async Task Handle()
            {
                // Just before commit save all the outgoing messages and generate their ids - for consistency.
                await publisher.outgoingEventRepository.SaveAsync(pendingSaveMessages.Select(x => x.GenerateOutgoingMessage(publisher.activityContext.CorrelationId, publisher.time.Now))).ConfigureAwait(false);
                publisher.pendingDispatchMessages.AddRange(pendingSaveMessages);

                // Execute any immediate message handlers so they are within the transaction
                foreach (var message in pendingSaveMessages)
                {
                    List<IObjectMessageProcessor> handlers;
                    if (immediateMessageHandlers.TryGetValue(message.MessageType, out handlers))
                    {
                        foreach (var handler in handlers)
                        {
                            publisher.messageStack.Push(new PendingMessageHandler(publisher));
                            try
                            {
                                await handler.Process(message);
                            }
                            finally
                            {
                                publisher.messageStack.Pop();
                            }
                        }
                    }
                }

                pendingSaveMessages.Clear();
            }

            private interface IObjectMessageProcessor
            {
                Task Process(object message);
            }

            private class ObjectToGenericMessageProcessor<TMessage> : IObjectMessageProcessor
            {
                private readonly IMessageProcessor<TMessage> processor;

                public ObjectToGenericMessageProcessor(IMessageProcessor<TMessage> processor)
                {
                    this.processor = processor;
                }

                public Task Process(object message)
                {
                    return processor.ProcessAsync((TMessage)message);
                }
            }
        }

        #endregion
    }
}
