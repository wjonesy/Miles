using MassTransit;
using MassTransit.Configurators;
using MassTransit.PipeBuilders;
using MassTransit.PipeConfigurators;
using Miles.MassTransit.Configuration;
using System.Collections.Generic;

namespace Miles.MassTransit.EnsureMessageDispatch
{
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
    /// <summary>
    /// Configures the <see cref="RecordMessageDispatchFilter{TContext}"/> .
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="MassTransit.PipeConfigurators.IPipeSpecification{TContext}" />
    /// <seealso cref="Miles.MassTransit.Configuration.IRecordMessageDispatchConfigurator" />
    class RecordMessageDispatchSpecification<TContext> : IPipeSpecification<TContext>, IRecordMessageDispatchConfigurator
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
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
