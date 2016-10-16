using MassTransit;
using MassTransit.ConsumeConfigurators;

namespace Miles.MassTransit.Configuration
{
    public interface IMessageProcessorMessageConfigurator<TMessage> : IPipeConfigurator<ConsumeContext<TMessage>>, IConsumeConfigurator
        where TMessage : class
    { }
}