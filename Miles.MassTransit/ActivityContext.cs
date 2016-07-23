using MassTransit;
using System;

namespace Miles.MassTransit
{
    public class ActivityContext : IActivityContext
    {
        public ActivityContext(ConsumeContext consumeContext)
        {
            CorrelationId = consumeContext?.CorrelationId ?? NewId.NextGuid();
        }

        public Guid CorrelationId { get; private set; }
    }
}
