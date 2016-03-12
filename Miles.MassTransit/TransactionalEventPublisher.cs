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
        private readonly IOutgoingEventRepository outgoingEventRepository;
        private readonly ITime time;
        private readonly ConsumeContext consumeContext;

        // State
        private readonly List<object> pendingSaveEvents = new List<object>();
        private List<OutgoingEventAndObject> pendingDispatchEvents;

        public TransactionalEventPublisher(
            ITransaction transaction,
            IOutgoingEventRepository outgoingEventRepository,
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
            // TODO: Id generation etc
            pendingDispatchEvents = pendingSaveEvents.Select(evt => new OutgoingEventAndObject(new OutgoingEvent(JsonConvert.SerializeObject(evt)), evt)).ToList();
            outgoingEventRepository.Save(pendingDispatchEvents.Select(x => x.OutgoingEvent));
            pendingSaveEvents.Clear();
        }

        private void Transaction_PostCommit(object sender, EventArgs e)
        {
            foreach (var evt in pendingDispatchEvents)
            {
                // TODO: Consider async
                // TODO: Id generation etc
                consumeContext.Publish(evt.EventObject,
                    callback: x =>
                    {
                        evt.OutgoingEvent.Dispatched(time);
                        outgoingEventRepository.Save(evt.OutgoingEvent, ignoreTransaction: true);
                    });
            }
        }

        public void Publish<TEvent>(TEvent evt) where TEvent : class
        {
            pendingSaveEvents.Add(evt);
        }

        private struct OutgoingEventAndObject
        {
            public OutgoingEventAndObject(OutgoingEvent outgoingEvent, Object eventObject)
            {
                this.OutgoingEvent = outgoingEvent;
                this.EventObject = eventObject;
            }

            public OutgoingEvent OutgoingEvent { get; private set; }
            public Object EventObject { get; private set; }
        }
    }
}
