using MassTransit;
using Miles.MassTransit.Configuration;
using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.TransactionContext;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorsConfigurator : IMessageProcessorsConfigurator
    {
        private readonly MessageProcessorOptions defaults = new MessageProcessorOptions();
        private readonly Dictionary<Type, IMessageProcessorConfigurator> processorConfigurators = new Dictionary<Type, IMessageProcessorConfigurator>();

        IMessageProcessorsConfigurator IMessageProcessorsConfigurator.UseTransactionContext(Action<ITransactionContextConfigurator> configure = null)
        {
            defaults.TransactionContext = new TransactionContextConfigurator();
            configure?.Invoke(defaults.TransactionContext);
            return this;
        }


        IMessageProcessorsConfigurator IMessageProcessorsConfigurator.UseMessageDeduplication(Action<IMessageDeduplicationConfigurator> configure = null)
        {
            defaults.MessageDeduplication = new MessageDeduplicationConfigurator();
            configure?.Invoke(defaults.MessageDeduplication);
            return this;
        }

        IMessageProcessorsConfigurator IMessageProcessorsConfigurator.ConfigureProcessor<TProcessor>(Action<IMessageProcessorConfigurator<TProcessor>> configure)
        {
            var configurator = new MessageProcessorConfigurator<TProcessor>();
            configure.Invoke(configurator);
            processorConfigurators[typeof(TProcessor)] = configurator;
            return this;
        }

        public IReceiveEndpointSpecification CreateEndpointSpecification(Type processorType, IConsumerFactoryFactory factory)
        {
            IMessageProcessorConfigurator configurator;
            if (!processorConfigurators.TryGetValue(processorType, out configurator))
                configurator = (IMessageProcessorConfigurator)Activator.CreateInstance(typeof(MessageProcessorConfigurator<>).MakeGenericType(processorType));
            return configurator.CreateSpecification(factory, defaults);
        }
    }
}
