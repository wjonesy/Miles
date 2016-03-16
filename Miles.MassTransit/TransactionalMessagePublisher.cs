using MassTransit;
using Miles.Events;
using Miles.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    public class TransactionalMessagePublisher : IEventPublisher, ICommandPublisher
    {
        private readonly IOutgoingMessageRepository outgoingEventRepository;
        private readonly ITime time;
        private readonly IBus bus;
        private readonly ConsumeContext consumeContext;

        // State
        private readonly List<Object> pendingSaveMessages = new List<Object>();
        private List<OutgoingMessageAndMessage> pendingDispatchMessages;

        public TransactionalMessagePublisher(
            ITransaction transaction,
            IOutgoingMessageRepository outgoingEventRepository,
            ITime time,
            IBus bus,
            ConsumeContext consumeContext = null)
        {
            this.outgoingEventRepository = outgoingEventRepository;
            this.bus = bus;
            this.consumeContext = consumeContext;
            this.time = time;

            transaction.PreCommit.Register(async (s, e) =>
            {
                pendingDispatchMessages = pendingSaveMessages
                    .Select(evt =>
                        new OutgoingMessageAndMessage(
                            new OutgoingMessage(
                                NewId.NextGuid(),
                                OutgoingMessageType.Event,
                                JsonConvert.SerializeObject(evt),
                                time),
                            evt)).ToList();
                await outgoingEventRepository.SaveAsync(pendingDispatchMessages.Select(x => x.OutgoingMessage));
                pendingSaveMessages.Clear();
            });

            // TODO: Remove from pendingDispatchMessages
            if (consumeContext == null)
            {
                // first event
                transaction.PostCommit.Register((s, e) =>
                    Task.WhenAll(pendingDispatchMessages.Select(evt => bus.Publish(evt.MessageObject, callback: x => x.MessageId = evt.OutgoingMessage.MessageId))));
            }
            else
            {
                // responding to an event or command
                transaction.PostCommit.Register((s, e) =>
                    Task.WhenAll(pendingDispatchMessages.Select(evt => consumeContext.Publish(evt.MessageObject, callback: x => x.MessageId = evt.OutgoingMessage.MessageId))));
            }
        }

        void IEventPublisher.Publish<TEvent>(TEvent evt)
        {
            pendingSaveMessages.Add(evt);
        }

        void ICommandPublisher.Publish<TCommand>(TCommand cmd)
        {
            pendingSaveMessages.Add(cmd);
        }

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
