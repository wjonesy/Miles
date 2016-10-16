using MassTransit;
using Miles.MassTransit.Configuration;

namespace Miles.MassTransit.ConsumerConvention
{
    interface IMessageProcessorConfigurator
    {
        IReceiveEndpointSpecification CreateSpecification(IConsumerFactoryFactory factory, MessageProcessorOptions defaults);
    }
}
