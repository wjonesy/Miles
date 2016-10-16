using Miles.Messaging;
using System;

namespace Miles.MassTransit.Configuration
{
    public interface IMessageProcessorsConfigurator
    {
        IMessageProcessorsConfigurator UseTransactionContext(Action<ITransactionContextConfigurator> configure = null);

        IMessageProcessorsConfigurator UseMessageDeduplication(Action<IMessageDeduplicationConfigurator> configure = null);

        IMessageProcessorsConfigurator ConfigureProcessor<TProcessor>(Action<IMessageProcessorConfigurator<TProcessor>> configure) where TProcessor : class, IMessageProcessor;
    }
}