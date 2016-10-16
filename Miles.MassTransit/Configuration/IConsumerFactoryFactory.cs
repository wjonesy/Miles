using MassTransit;

namespace Miles.MassTransit.Configuration
{
    public interface IConsumerFactoryFactory
    {
        IConsumerFactory<TConsumer> CreateConsumerFactory<TConsumer>() where TConsumer : class;
    }
}