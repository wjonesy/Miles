using MassTransit;
using MassTransit.PipeConfigurators;
using Miles.Messaging;
using System.Collections.Generic;

namespace Miles.MassTransit.ConsumerConvention
{
    interface IMessageProcessorMessageConfigurator
    {
        IEnumerable<IPipeSpecification<ConsumerConsumeContext<TProcessor>>> CreateSpecifications<TProcessor>(MessageProcessorOptions newDefaults)
            where TProcessor : class, IMessageProcessor;
    }
}
