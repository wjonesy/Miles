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
    // TODO: Consider async
    public class TransactionalMessagePublisher : IEventPublisher, ICommandPublisher
    {
        private readonly IOutgoingMessageRepository outgoingEventRepository;
        private readonly ITime time;
        private readonly ConsumeContext consumeContext;

        // State
        private readonly List<IMilesMassTransitEnvelope<Object>> pendingSaveMessages = new List<IMilesMassTransitEnvelope<Object>>();
        private List<OutgoingMessageAndMessage> pendingDispatchMessages;

        public TransactionalMessagePublisher(
            ITransaction transaction,
            IOutgoingMessageRepository outgoingEventRepository,
            ITime time,
            ConsumeContext consumeContext)
        {
            this.outgoingEventRepository = outgoingEventRepository;
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

            transaction.PostCommit.Register((s, e) =>
                Task.WhenAll(pendingDispatchMessages.Select(evt => consumeContext.Publish(
                    evt.MessageObject,
                    callback: x =>
                    {
                        evt.OutgoingMessage.Dispatched(time);
                        return outgoingEventRepository.SaveAsync(evt.OutgoingMessage, ignoreTransaction: true);
                    }))));
        }

        void IEventPublisher.Publish<TEvent>(TEvent evt)
        {
            var eventMessage = new MilesMassTransitEnvelope<TEvent>(NewId.NextGuid(), evt);
            pendingSaveMessages.Add(eventMessage);
        }

        void ICommandPublisher.Publish<TCommand>(TCommand cmd)
        {
            var eventMessage = new MilesMassTransitEnvelope<TCommand>(NewId.NextGuid(), cmd);
            pendingSaveMessages.Add(eventMessage);
        }

        private struct OutgoingMessageAndMessage
        {
            public OutgoingMessageAndMessage(OutgoingMessage outgoingMessage, IMilesMassTransitEnvelope<Object> messageObject)
            {
                this.OutgoingMessage = outgoingMessage;
                this.MessageObject = messageObject;
            }

            public OutgoingMessage OutgoingMessage { get; private set; }
            public IMilesMassTransitEnvelope<Object> MessageObject { get; private set; }
        }
    }
}
