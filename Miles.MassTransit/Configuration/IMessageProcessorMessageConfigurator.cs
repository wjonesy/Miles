using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.PipeConfigurators;
using Miles.MassTransit.TransactionContext;
using Miles.Messaging;
using System.Collections.Generic;

namespace Miles.MassTransit.Configuration
{
    interface IMessageProcessorMessageConfigurator
    {
        IEnumerable<IPipeSpecification<ConsumerConsumeContext<TProcessor>>> GetSpecifications<TProcessor>(
            TransactionContextConfigurator transactionContextDefaults = null)
            where TProcessor : class, IMessageProcessor;
    }

    public interface IMessageProcessorMessageConfigurator<TMessage> : IPipeConfigurator<ConsumeContext<TMessage>>, IConsumeConfigurator
        where TMessage : class
    { }
}