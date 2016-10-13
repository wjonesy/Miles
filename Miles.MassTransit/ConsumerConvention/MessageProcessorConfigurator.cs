using MassTransit;
using MassTransit.PipeConfigurators;
using Miles.MassTransit.Configuration;
using Miles.MassTransit.TransactionContext;
using Miles.Messaging;
using Miles.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorConfigurator<TProcessor> : IMessageProcessorConfigurator<TProcessor>
        where TProcessor : class, IMessageProcessor
    {
        private readonly List<IPipeSpecification<ConsumerConsumeContext<TProcessor>>> specifications = new List<IPipeSpecification<ConsumerConsumeContext<TProcessor>>>();
        private TransactionContextConfigurator transactionContextConfig;
        private readonly Dictionary<Type, IMessageProcessorMessageConfigurator> messageSpecifications = new Dictionary<Type, IMessageProcessorMessageConfigurator>();

        public void AddPipeSpecification(IPipeSpecification<ConsumerConsumeContext<TProcessor>> specification)
        {
            specifications.Add(specification);
        }

        public IMessageProcessorConfigurator<TProcessor> UseTransactionContext(Action<ITransactionContextConfigurator> configure = null)
        {
            transactionContextConfig = new TransactionContextConfigurator();
            configure?.Invoke(transactionContextConfig);
            return this;
        }

        public IMessageProcessorConfigurator<TProcessor> ConfigureMessage<TMessage>(Action<IMessageProcessorMessageConfigurator<TMessage>> configure) where TMessage : class
        {
            var messageConfigurator = new MessageProcessorMessageConfigurator<TMessage>();
            configure.Invoke(messageConfigurator);
            messageSpecifications[typeof(TMessage)] = messageConfigurator;
            return this;
        }

        public IEnumerable<IPipeSpecification<ConsumerConsumeContext<TProcessor>>> GetSpecifications()
        {
            foreach (var spec in specifications)
                yield return spec;

            foreach (var messageSpec in messageSpecifications.Values.SelectMany(x => x.GetSpecifications<TProcessor>()))
                yield return messageSpec;

            var messageTypesLackingConfig = typeof(TProcessor).GetInterfaces()
                .Where(x => x.IsMessageProcessor())
                .Select(x => x.GetGenericArguments().First())
                .Except(messageSpecifications.Keys);

            foreach (var messageType in messageTypesLackingConfig)
            {
                var messageConfiguratorType = typeof(MessageProcessorMessageConfigurator<>).MakeGenericType(messageType);
                var messageConfiguratorInstance = (IMessageProcessorMessageConfigurator)Activator.CreateInstance(messageConfiguratorType);
                foreach (var messageSpec in messageConfiguratorInstance.GetSpecifications<TProcessor>(transactionContextConfig))
                    yield return messageSpec;
            }
        }
    }
}