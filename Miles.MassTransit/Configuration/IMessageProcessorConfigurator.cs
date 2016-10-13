using MassTransit;
using MassTransit.ConsumeConfigurators;
using Miles.Messaging;
using System;

namespace Miles.MassTransit.Configuration
{
    public interface IMessageProcessorConfigurator<TProcessor> : IPipeConfigurator<ConsumerConsumeContext<TProcessor>>, IConsumeConfigurator
        where TProcessor : class, IMessageProcessor
    {
        IMessageProcessorConfigurator<TProcessor> UseTransactionContext(Action<ITransactionContextConfigurator> configure = null);

        IMessageProcessorConfigurator<TProcessor> ConfigureMessage<TMessage>(Action<IMessageProcessorMessageConfigurator<TMessage>> configure) where TMessage : class;
    }
}