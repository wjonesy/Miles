using MassTransit;
using MassTransit.Configurators;
using MassTransit.PipeBuilders;
using MassTransit.PipeConfigurators;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.EnsureMessageDispatch
{
    class RecordMessageDispatchSpecification : IPipeSpecification<SendContext>
    {
        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<SendContext> builder)
        {
            builder.AddFilter(new RecordMessageDispatchFilter());
        }
    }

    class RecordMessageDispatchSpecification<TMessage> : IPipeSpecification<SendContext<TMessage>> where TMessage : class
    {
        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<SendContext<TMessage>> builder)
        {
            builder.AddFilter(new RecordMessageDispatchFilter<TMessage>());
        }
    }
}
