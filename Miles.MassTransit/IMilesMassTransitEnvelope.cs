using System;

namespace Miles.MassTransit
{
    public interface IMilesMassTransitEnvelope<out TMessage> where TMessage : class
    {
        Guid TransactionMessageId { get; }
        TMessage Message { get; }
    }
}
