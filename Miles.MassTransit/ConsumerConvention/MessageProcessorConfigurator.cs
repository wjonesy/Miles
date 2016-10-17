using MassTransit;
using MassTransit.PipeConfigurators;
using Miles.MassTransit.Configuration;
using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.TransactionContext;
using Miles.Messaging;
using Miles.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorConfigurator<TProcessor> : IMessageProcessorConfigurator<TProcessor>, IMessageProcessorConfigurator
        where TProcessor : class, IMessageProcessor
    {
        private readonly List<IPipeSpecification<ConsumerConsumeContext<TProcessor>>> specifications = new List<IPipeSpecification<ConsumerConsumeContext<TProcessor>>>();
        private readonly MessageProcessorOptions options = new MessageProcessorOptions();
        private readonly Dictionary<Type, IMessageProcessorMessageConfigurator> messageConfigurators = new Dictionary<Type, IMessageProcessorMessageConfigurator>();

        public void AddPipeSpecification(IPipeSpecification<ConsumerConsumeContext<TProcessor>> specification)
        {
            specifications.Add(specification);
        }

        public IMessageProcessorConfigurator<TProcessor> UseTransactionContext(Action<ITransactionContextConfigurator> configure = null)
        {
            options.TransactionContext = new TransactionContextConfigurator();
            configure?.Invoke(options.TransactionContext);
            return this;
        }


        public IMessageProcessorConfigurator<TProcessor> UseMessageDeduplication(Action<IMessageDeduplicationConfigurator> configure = null)
        {
            options.MessageDeduplication = new MessageDeduplicationConfigurator();
            configure?.Invoke(options.MessageDeduplication);
            return this;
        }

        public IMessageProcessorConfigurator<TProcessor> ConfigureMessage<TMessage>(Action<IMessageProcessorMessageConfigurator<TMessage>> configure) where TMessage : class
        {
            var messageConfigurator = new MessageProcessorMessageConfigurator<TMessage>();
            configure.Invoke(messageConfigurator);
            messageConfigurators[typeof(TMessage)] = messageConfigurator;
            return this;
        }

        public IReceiveEndpointSpecification CreateSpecification(IConsumerFactoryFactory factory, MessageProcessorOptions defaults)
        {
            return CreateSpecification(factory.CreateConsumerFactory<TProcessor>(), defaults);
        }

        public IReceiveEndpointSpecification CreateSpecification(IConsumerFactory<TProcessor> factory, MessageProcessorOptions defaults)
        {
            return new MessageProcessorSpecification<TProcessor>(factory, CreateInternalSpecifications(defaults).ToArray());
        }

        private IEnumerable<IPipeSpecification<ConsumerConsumeContext<TProcessor>>> CreateInternalSpecifications(MessageProcessorOptions defaults)
        {
            // create new defaults based on configuration and supplied defaults
            var newDefaults = options.Merge(defaults);

            foreach (var spec in specifications)
                yield return spec;

            foreach (var messageSpec in messageConfigurators.Values.SelectMany(x => x.CreateSpecifications<TProcessor>(newDefaults)))
                yield return messageSpec;

            // Create remaining message types based on defaults
            var messageTypesLackingConfig = typeof(TProcessor).GetInterfaces()
                .Where(x => x.IsMessageProcessor())
                .Select(x => x.GetGenericArguments().First())
                .Except(messageConfigurators.Keys);

            foreach (var messageType in messageTypesLackingConfig)
            {
                var messageConfiguratorType = typeof(MessageProcessorMessageConfigurator<>).MakeGenericType(messageType);
                var messageConfiguratorInstance = (IMessageProcessorMessageConfigurator)Activator.CreateInstance(messageConfiguratorType);

                foreach (var messageSpec in messageConfiguratorInstance.CreateSpecifications<TProcessor>(newDefaults))
                    yield return messageSpec;
            }
        }
    }
}