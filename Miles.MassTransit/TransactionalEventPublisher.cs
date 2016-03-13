using MassTransit;
using Miles.Events;
using Miles.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit
{
    // TODO: Consider async
    public class TransactionalEventPublisher : IEventPublisher
    {
        private readonly IOutgoingMessageRepository outgoingEventRepository;
        private readonly ITime time;
        private readonly ConsumeContext consumeContext;

        // State
        private readonly List<IMilesMassTransitEnvelope<Object>> pendingSaveEvents = new List<IMilesMassTransitEnvelope<Object>>();
        private List<OutgoingMessageAndMessage> pendingDispatchEvents;

        public TransactionalEventPublisher(
            ITransaction transaction,
            IOutgoingMessageRepository outgoingEventRepository,
            ITime time,
            ConsumeContext consumeContext)
        {
            this.outgoingEventRepository = outgoingEventRepository;
            this.consumeContext = consumeContext;
            this.time = time;

            transaction.PreCommit += Transaction_PreCommit;
            transaction.PostCommit += Transaction_PostCommit;
        }

        private void Transaction_PreCommit(object sender, EventArgs e)
        {
            pendingDispatchEvents = pendingSaveEvents
                .Select(evt =>
                    new OutgoingMessageAndMessage(
                        new OutgoingMessage(
                            NewId.NextGuid(),
                            OutgoingMessageType.Event,
                            JsonConvert.SerializeObject(evt),
                            time),
                        evt)).ToList();
            outgoingEventRepository.Save(pendingDispatchEvents.Select(x => x.OutgoingMessage));
            pendingSaveEvents.Clear();
        }

        private void Transaction_PostCommit(object sender, EventArgs e)
        {
            foreach (var evt in pendingDispatchEvents)
            {
                // TODO: Consider async
                consumeContext.Publish(
                    evt.EventObject,
                    callback: x =>
                    {
                        evt.OutgoingMessage.Dispatched(time);
                        outgoingEventRepository.Save(evt.OutgoingMessage, ignoreTransaction: true);
                    });
            }
        }

        public void Publish<TEvent>(TEvent evt) where TEvent : class
        {
            var eventMessage = new MilesMassTransitEnvelope<TEvent>(NewId.NextGuid(), evt);
            pendingSaveEvents.Add(eventMessage);
        }

        private struct OutgoingMessageAndMessage
        {
            public OutgoingMessageAndMessage(OutgoingMessage outgoingEvent, IMilesMassTransitEnvelope<Object> eventObject)
            {
                this.OutgoingMessage = outgoingEvent;
                this.EventObject = eventObject;
            }

            public OutgoingMessage OutgoingMessage { get; private set; }
            public IMilesMassTransitEnvelope<Object> EventObject { get; private set; }
        }
    }
}
