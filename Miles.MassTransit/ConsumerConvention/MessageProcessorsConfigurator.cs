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
        #region Configurator

        private readonly MessageProcessorOptions options = new MessageProcessorOptions();
        private readonly Dictionary<Type, IMessageProcessorConfigurator> processorConfigurators = new Dictionary<Type, IMessageProcessorConfigurator>();

        IMessageProcessorsConfigurator IMessageProcessorsConfigurator.UseTransactionContext(Action<ITransactionContextConfigurator> configure)
        {
            options.TransactionContext = new TransactionContextConfigurator();
            configure?.Invoke(options.TransactionContext);
            return this;
        }


        IMessageProcessorsConfigurator IMessageProcessorsConfigurator.UseMessageDeduplication(Action<IMessageDeduplicationConfigurator> configure)
        {
            options.MessageDeduplication = new MessageDeduplicationConfigurator();
            configure?.Invoke(options.MessageDeduplication);
            return this;
        }

        IMessageProcessorsConfigurator IMessageProcessorsConfigurator.ConfigureProcessor<TProcessor>(Action<IMessageProcessorConfigurator<TProcessor>> configure)
        {
            var configurator = new MessageProcessorConfigurator<TProcessor>();
            configure.Invoke(configurator);
            processorConfigurators[typeof(TProcessor)] = configurator;
            return this;
        }

        #endregion

        public IReceiveEndpointSpecification CreateEndpointSpecification(Type processorType, IConsumerFactoryFactory factory)
        {
            IMessageProcessorConfigurator configurator;
            if (!processorConfigurators.TryGetValue(processorType, out configurator))
                configurator = (IMessageProcessorConfigurator)Activator.CreateInstance(typeof(MessageProcessorConfigurator<>).MakeGenericType(processorType));
            return configurator.CreateSpecification(factory, options);
        }
    }
}
