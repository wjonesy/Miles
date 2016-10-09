using MassTransit;
using MassTransit.Configurators;
using MassTransit.ConsumeConfigurators;
using MassTransit.ConsumeConnectors;
using MassTransit.PipeConfigurators;
using Miles.Messaging;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.Configuration
{
    class MessageProcessorConfigurator<TProcessor> : IMessageProcessorConfigurator<TProcessor>, IReceiveEndpointSpecification
        where TProcessor : class, IMessageProcessor
    {
        private readonly List<IPipeSpecification<ConsumerConsumeContext<TProcessor>>> specifications = new List<IPipeSpecification<ConsumerConsumeContext<TProcessor>>>();

        private readonly IConsumerFactory<TProcessor> consumerFactory;

        public MessageProcessorConfigurator(IConsumerFactory<TProcessor> consumerFactory)
        {
            this.consumerFactory = consumerFactory;
        }

        public void AddPipeSpecification(IPipeSpecification<ConsumerConsumeContext<TProcessor>> specification)
        {
            specifications.Add(specification);
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return consumerFactory.Validate().Concat(specifications.SelectMany(x => x.Validate()));
        }

        public void Configure(IReceiveEndpointBuilder builder)
        {
            ConsumerConnectorCache<TProcessor>.Connector.ConnectConsumer(builder, consumerFactory, specifications.ToArray());
        }
    }
}