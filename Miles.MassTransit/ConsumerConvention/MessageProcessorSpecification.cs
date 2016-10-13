using MassTransit;
using MassTransit.Configurators;
using MassTransit.ConsumeConfigurators;
using MassTransit.ConsumeConnectors;
using MassTransit.PipeConfigurators;
using Miles.Messaging;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorSpecification<TProcessor> : IReceiveEndpointSpecification
        where TProcessor : class, IMessageProcessor
    {
        private readonly IConsumerFactory<TProcessor> consumerFactory;
        private readonly IPipeSpecification<ConsumerConsumeContext<TProcessor>>[] specifications;

        public MessageProcessorSpecification(IConsumerFactory<TProcessor> consumerFactory, IPipeSpecification<ConsumerConsumeContext<TProcessor>>[] specifications)
        {
            this.consumerFactory = consumerFactory;
            this.specifications = specifications;
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
