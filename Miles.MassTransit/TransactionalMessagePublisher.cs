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
        private HashSet<OutgoingMessageAndObject> pendingSaveMessages = new HashSet<OutgoingMessageAndObject>();
        private HashSet<OutgoingMessageAndObject> pendingDispatchMessages = new HashSet<OutgoingMessageAndObject>();
        // handlers
        private readonly Dictionary<Type, List<IObjectMessageProcessor>> immediateMessageHandlers = new Dictionary<Type, List<IObjectMessageProcessor>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionalMessagePublisher" /> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="consumeContext">The consume context.</param>
        /// <param name="outgoingEventRepository">The outgoing event repository.</param>
        /// <param name="time">The time.</param>
        /// <param name="commandDispatcher">The command dispatcher.</param>
        /// <param name="eventDispatcher">The event dispatcher.</param>
        public TransactionalMessagePublisher(
            ITransaction transaction,
            ConsumeContext consumeContext,
            IOutgoingMessageRepository outgoingEventRepository,
            ITime time,
            IMessageDispatcher commandDispatcher,
            ConventionBasedMessageDispatcher eventDispatcher)
        {
            this.outgoingEventRepository = outgoingEventRepository;
            this.time = time;

            var correlationId = consumeContext?.CorrelationId ?? NewId.NextGuid();  // TODO, maybe want to centerlize this for easy logging etc

            transaction.PreCommit.Register(async (s, e) =>
            {
                // Just before commit save all the outgoing messages and generate their ids.
                await outgoingEventRepository.SaveAsync(pendingSaveMessages.Select(x => x.GenerateOutgoingMessage(correlationId, time.Now))).ConfigureAwait(false);
                pendingDispatchMessages = pendingSaveMessages;
                pendingSaveMessages = new HashSet<OutgoingMessageAndObject>();

                // Execute any immediate message handlers so they are within transaction
                foreach (var message in pendingDispatchMessages)
                {
                    List<IObjectMessageProcessor> handlers;
                    if (immediateMessageHandlers.TryGetValue(message.MessageType, out handlers))
                    {
                        foreach (var handler in handlers)
                            await handler.Process(message);
                    }
                }
            });

            // After commit dispatch the messages. Try to mark them as dispatched.
            transaction.PostCommit.Register(async (s, e) =>
            {
                foreach (var message in pendingDispatchMessages)
                {
                    var dispatcher = message.OutgoingMessage.MessageType == OutgoingMessageType.Command ? commandDispatcher : eventDispatcher;
                    await dispatcher.DispatchAsync(message.MessageObject, message.OutgoingMessage).ConfigureAwait(false);
                    message.OutgoingMessage.Dispatched(time.Now);
                    await outgoingEventRepository.SaveAsync(message.OutgoingMessage, ignoreTransaction: true).ConfigureAwait(false);
                }
                pendingDispatchMessages.Clear();
            });
        }

        #region IEventPublisher

        void IEventPublisher.Register<TEvent>(IMessageProcessor<TEvent> evt)
        {
            AddMessageHandler(evt);
        }

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="evt">The event.</param>
        void IEventPublisher.Publish<TEvent>(TEvent evt)
        {
            pendingSaveMessages.Add(new OutgoingMessageAndObject(OutgoingMessageType.Event, typeof(TEvent), evt));
        }

        #endregion

        #region ICommandPublisher

        void ICommandPublisher.Register<TCommand>(IMessageProcessor<TCommand> cmd)
        {
            AddMessageHandler(cmd);
        }

        /// <summary>
        /// Publishes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="cmd">The command.</param>
        void ICommandPublisher.Publish<TCommand>(TCommand cmd)
        {
            pendingSaveMessages.Add(new OutgoingMessageAndObject(OutgoingMessageType.Command, typeof(TCommand), cmd));
        }

        #endregion

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
                    OutgoingMessage = new OutgoingMessage(NewId.NextGuid(), correlationId, OutgoingMessageType, JsonConvert.SerializeObject(MessageObject), eventCreated);

                return OutgoingMessage;
            }
        }

        #region Register and calling message handlers immediately

        private void AddMessageHandler<TMessage>(IMessageProcessor<TMessage> messageHandler) where TMessage : class
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

        #endregion
    }
}
