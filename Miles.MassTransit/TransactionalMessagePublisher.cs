using MassTransit;
using Miles.Events;
using Miles.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private readonly List<Object> pendingSaveEvents = new List<Object>();
        private readonly List<Object> pendingSaveCommands = new List<Object>();
        private List<OutgoingMessageAndMessage> pendingDispatchMessages;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionalMessagePublisher"/> class.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="outgoingEventRepository">The outgoing event repository.</param>
        /// <param name="time">The time.</param>
        /// <param name="commandDispatcher">The command dispatcher.</param>
        /// <param name="eventDispatcher">The event dispatcher.</param>
        public TransactionalMessagePublisher(
            ITransaction transaction,
            IOutgoingMessageRepository outgoingEventRepository,
            ITime time,
            IMessageDispatcher commandDispatcher,
            ConventionBasedMessageDispatcher eventDispatcher)
        {
            this.outgoingEventRepository = outgoingEventRepository;
            this.time = time;

            // Just before commit save all the outgoing messages and generate their ids.
            transaction.PreCommit.Register(async (s, e) =>
            {
                pendingDispatchMessages = pendingSaveEvents
                    .Select(evt =>
                        new OutgoingMessageAndMessage(
                            new OutgoingMessage(
                                NewId.NextGuid(),
                                OutgoingMessageType.Event,
                                JsonConvert.SerializeObject(evt),
                                time),
                            evt))
                    .Concat(pendingSaveCommands.Select(evt =>
                        new OutgoingMessageAndMessage(
                            new OutgoingMessage(
                                NewId.NextGuid(),
                                OutgoingMessageType.Command,
                                JsonConvert.SerializeObject(evt),
                                time),
                            evt))).ToList();
                await outgoingEventRepository.SaveAsync(pendingDispatchMessages.Select(x => x.OutgoingMessage));
                pendingSaveEvents.Clear();
                pendingSaveCommands.Clear();
            });

            // After commit dispatch the messages. Try to mark them as dispatched.
            transaction.PostCommit.Register(async (s, e) =>
            {
                foreach (var message in pendingDispatchMessages.ToList())
                {
                    var dispatcher = message.OutgoingMessage.MessageType == OutgoingMessageType.Command ? commandDispatcher : eventDispatcher;
                    await dispatcher.DispatchAsync(message.MessageObject, message.OutgoingMessage.MessageId);
                    message.OutgoingMessage.Dispatched(time);
                    await outgoingEventRepository.SaveAsync(message.OutgoingMessage, ignoreTransaction: true);
                    pendingDispatchMessages.Remove(message);
                }
            });
        }

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="evt">The event.</param>
        void IEventPublisher.Publish<TEvent>(TEvent evt)
        {
            pendingSaveEvents.Add(evt);
        }

        /// <summary>
        /// Publishes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="cmd">The command.</param>
        void ICommandPublisher.Publish<TCommand>(TCommand cmd)
        {
            pendingSaveCommands.Add(cmd);
        }

        // Internal structure to keep the message object and its db representation together
        private struct OutgoingMessageAndMessage
        {
            public OutgoingMessageAndMessage(OutgoingMessage outgoingMessage, Object messageObject)
            {
                this.OutgoingMessage = outgoingMessage;
                this.MessageObject = messageObject;
            }

            public OutgoingMessage OutgoingMessage { get; private set; }
            public Object MessageObject { get; private set; }
        }
    }
}
