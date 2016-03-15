using System;

namespace Miles.MassTransit
{
    interface IMilesMassTransitEnvelope<out TMessage> where TMessage : class
    {
        Guid TransactionMessageId { get; }
        TMessage Message { get; }
    }
}
