using MassTransit;
using MassTransit.PipeConfigurators;
using Miles.Messaging;
using System.Collections.Generic;

namespace Miles.MassTransit.ConsumerConvention
{
    /// <summary>
    /// Helps create specifications from without needing to know the specific message type.
    /// </summary>
    interface IMessageProcessorMessageConfigurator
    {
        IEnumerable<IPipeSpecification<ConsumerConsumeContext<TProcessor>>> CreateSpecifications<TProcessor>(MessageProcessorOptions newDefaults)
            where TProcessor : class, IMessageProcessor;
    }
}
