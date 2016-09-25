using MassTransit;
using MassTransit.Configurators;
using MassTransit.PipeBuilders;
using MassTransit.PipeConfigurators;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.MessageDeduplication
{
    class MessageDeduplicationSpecification<TConsumer> : IPipeSpecification<ConsumerConsumeContext<TConsumer>> where TConsumer : class, IConsumer
    {
        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<ConsumerConsumeContext<TConsumer>> builder)
        {
            builder.AddFilter(new MessageDeduplicationFilter<TConsumer>());
        }
    }
}
