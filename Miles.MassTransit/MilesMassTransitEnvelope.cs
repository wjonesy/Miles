using System;

namespace Miles.MassTransit
{
    class MilesMassTransitEnvelope<TMessage> : IMilesMassTransitEnvelope<TMessage> where TMessage : class
    {
        public MilesMassTransitEnvelope(
            Guid transactionMessageId,
            TMessage message)
        {
            this.TransactionMessageId = transactionMessageId;
            this.Message = message;
        }

        public Guid TransactionMessageId { get; private set; }

        public TMessage Message { get; private set; }
    }
}
