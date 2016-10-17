using MassTransit;
using Miles.MassTransit.Configuration;

namespace Miles.MassTransit.ConsumerConvention
{
    /// <summary>
    /// Helps create specifications from without needing to know the specific processor type.
    /// </summary>
    interface IMessageProcessorConfigurator
    {
        IReceiveEndpointSpecification CreateSpecification(IConsumerFactoryFactory factory, MessageProcessorOptions defaults);
    }
}
