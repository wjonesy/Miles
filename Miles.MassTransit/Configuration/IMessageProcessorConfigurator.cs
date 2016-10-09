using MassTransit;
using MassTransit.ConsumeConfigurators;
using Miles.Messaging;

namespace Miles.MassTransit.Configuration
{
    public interface IMessageProcessorConfigurator<TProcessor> : IPipeConfigurator<ConsumerConsumeContext<TProcessor>>, IConsumeConfigurator
        where TProcessor : class, IMessageProcessor
    { }
}