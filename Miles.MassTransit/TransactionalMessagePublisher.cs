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
    /// within a data store, subject to the transaction, with consistant message identifiers to aid
    /// in message de-duplication.
    /// </summary>
    /// <seealso cref="Messaging.IEventPublisher" />
    /// <seealso cref="Messaging.ICommandPublisher" />
    public class TransactionalMessagePublisher : IEventPublisher, ICommandPublisher
    {
        private readonly IOutgoingMessageRepository outgoingMessageRepository;
        private readonly ITime time;

        // State
        private readonly Stack<PublisherStackInstance> publisherStack = new Stack<PublisherStackInstance>();
        private List<OutgoingMessageForDispatch> pendingDispatchMessages = new List<OutgoingMessageForDispatch>();
        private readonly IActivityContext activityContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionalMessagePublisher" /> class.
        /// </summary>
        /// <param name="transactionContext">The transaction context.</param>
        /// <param name="outgoingMessageRepository">The outgoing message repository.</param>
        /// <param name="time">The time service.</param>
        /// <param name="activityContext">The activity context.</param>
        /// <param name="messageDispatchProcess">The message dispatch process.</param>
        public TransactionalMessagePublisher(
            ITransactionContext transactionContext,
            IOutgoingMessageRepository outgoingMessageRepository,
            ITime time,
            IActivityContext activityContext,
            IMessageDispatchProcess messageDispatchProcess)
        {
            this.publisherStack.Push(new PublisherStackInstance(this));
            this.outgoingMessageRepository = outgoingMessageRepository;
            this.time = time;
            this.activityContext = activityContext;

            transactionContext.PreCommit.Register((s, e) => publisherStack.Peek().Process());

            transactionContext.PostCommit.Register(async (s, e) =>
            {
                // relinquish control of the collection, let the dispatcher process own it
                var messagesForDispatch = pendingDispatchMessages;
                pendingDispatchMessages = new List<OutgoingMessageForDispatch>();

                await messageDispatchProcess.ExecuteAsync(messagesForDispatch).ConfigureAwait(false);
            });
        }

        #region IEventPublisher

        void IEventPublisher.Register<TEvent>(IMessageProcessor<TEvent> evt)
        {
            publisherStack.Peek().Register(evt);
        }

        void IEventPublisher.Publish<TEvent>(TEvent evt)
        {
            publisherStack.Peek().Publish(OutgoingMessageConceptType.Event, evt);
        }

        #endregion

        #region ICommandPublisher

        void ICommandPublisher.Register<TCommand>(IMessageProcessor<TCommand> cmd)
        {
            publisherStack.Peek().Register(cmd);
        }

        void ICommandPublisher.Publish<TCommand>(TCommand cmd)
        {
            publisherStack.Peek().Publish(OutgoingMessageConceptType.Command, cmd);
        }

        #endregion

        #region Register and calling message processors immediately

        /// <summary>
        /// As we call immediate message processors we create a stack of scopes. This is so processors don't register other processors multiple times (each registering the same)
        /// as each scope has its own set of registered processors. I think it also feels conceptually more logical, less unexpected corner cases.
        /// </summary>
        private class PublisherStackInstance
        {
            private readonly TransactionalMessagePublisher publicPublisher;
            private List<OutgoingMessageForDispatch> pendingSaveMessages = new List<OutgoingMessageForDispatch>();
            private readonly Dictionary<Type, List<IObjectMessageProcessor>> immediateMessageProcessors = new Dictionary<Type, List<IObjectMessageProcessor>>();

            public PublisherStackInstance(TransactionalMessagePublisher publisher)
            {
                this.publicPublisher = publisher;
            }

            public void Register<TMessage>(IMessageProcessor<TMessage> messageHandler) where TMessage : class
            {
                var messageType = typeof(TMessage);

                List<IObjectMessageProcessor> processors;
                if (!immediateMessageProcessors.TryGetValue(messageType, out processors))
                {
                    processors = new List<IObjectMessageProcessor>();
                    immediateMessageProcessors.Add(messageType, processors);
                }

                processors.Add(new ObjectToGenericMessageProcessor<TMessage>(messageHandler));
            }

            public void Publish<TMessage>(OutgoingMessageConceptType type, TMessage msg)
            {
                pendingSaveMessages.Add(new OutgoingMessageForDispatch(type, typeof(TMessage), msg, NewId.NextGuid(), publicPublisher.activityContext.CorrelationId));
            }

            public async Task Process()
            {
                var processingMessages = pendingSaveMessages;
                pendingSaveMessages = new List<OutgoingMessageForDispatch>();

                // Just before commit save all the outgoing messages and their ids were already generated - for consistency.
                var currentTime = publicPublisher.time.Now;
                var outgoingMessages = processingMessages.Select(x => new OutgoingMessage(
                    x.MessageId,
                    x.CorrelationId,
                    x.MessageType.FullName,
                    x.ConceptType,
                    JsonConvert.SerializeObject(x.MessageObject),
                    currentTime));
                await publicPublisher.outgoingMessageRepository.SaveAsync(outgoingMessages).ConfigureAwait(false);

                // Transition messages ready for dispatch
                publicPublisher.pendingDispatchMessages.AddRange(processingMessages);

                // Execute any immediate message processors so they are within the transaction
                foreach (var message in processingMessages)
                {
                    List<IObjectMessageProcessor> processors;
                    if (immediateMessageProcessors.TryGetValue(message.MessageType, out processors))
                    {
                        foreach (var processor in processors)
                        {
                            // lets get recursive
                            publicPublisher.publisherStack.Push(new PublisherStackInstance(publicPublisher));
                            try
                            {
                                await processor.Process(message.MessageObject).ConfigureAwait(false);
                            }
                            finally
                            {
                                // and recurse out
                                publicPublisher.publisherStack.Pop();
                            }
                        }
                    }
                }
            }

            private interface IObjectMessageProcessor
            {
                Task Process(object message);
            }

            // Allows us to return the message reference to the real type reference ready for calling the processor
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
