using MassTransit;
using MassTransit.Configurators;
using MassTransit.PipeBuilders;
using MassTransit.PipeConfigurators;
using Miles.MassTransit.Configuration;
using System.Collections.Generic;

namespace Miles.MassTransit.EnsureMessageDispatch
{
    class RecordMessageDispatchSpecification<TContext> : IPipeSpecification<TContext>, IRecordMessageDispatchConfigurator
        where TContext : class, SendContext
    {
        public IDispatchedRepository DispatchedRepository { get; private set; }

        public IRecordMessageDispatchConfigurator UseDispatchedRepository(IDispatchedRepository repository)
        {
            this.DispatchedRepository = repository;
            return this;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            if (DispatchedRepository == null)
                yield return new ConfigurationValidationResult(
                    ValidationResultDisposition.Failure,
                    "DispatchedRepository",
                    "Cannot be null",
                    DispatchedRepository.ToString());
        }

        public void Apply(IPipeBuilder<TContext> builder)
        {
            builder.AddFilter(new RecordMessageDispatchFilter<TContext>(DispatchedRepository));
        }
    }
}
