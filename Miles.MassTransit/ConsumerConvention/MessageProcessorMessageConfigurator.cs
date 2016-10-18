using MassTransit;
using MassTransit.Configurators;
using MassTransit.Internals.Extensions;
using MassTransit.PipeBuilders;
using MassTransit.PipeConfigurators;
using MassTransit.Util;
using Miles.MassTransit.Configuration;
using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.TransactionContext;
using Miles.Messaging;
using Miles.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorMessageConfigurator<TMessage> : IMessageProcessorMessageConfigurator<TMessage>, IMessageProcessorMessageConfigurator
        where TMessage : class
    {
        #region Configurator

        private readonly List<IPipeSpecification<ConsumeContext<TMessage>>> specifications = new List<IPipeSpecification<ConsumeContext<TMessage>>>();

        private bool transactionConfigurationOverriden = false;
        private bool messageDeduplicationOverride = false;

        public void AddPipeSpecification(IPipeSpecification<ConsumeContext<TMessage>> specification)
        {
            if ((specification as TransactionContextConfigurator) != null)
                transactionConfigurationOverriden = true;
            else if ((specification as MessageDeduplicationConfigurator) != null)
                messageDeduplicationOverride = true;

            specifications.Add(specification);
        }

        #endregion

        #region Create specifications

        public IEnumerable<IPipeSpecification<ConsumerConsumeContext<TProcessor>>> CreateSpecifications<TProcessor>(MessageProcessorOptions defaults)
            where TProcessor : class, IMessageProcessor
        {
            var processMethod = typeof(TProcessor).GetInterfaceMap(typeof(IMessageProcessor<TMessage>)).TargetMethods.Single();

            if (!transactionConfigurationOverriden)
            {
                var config = GetConfig(
                    a => new TransactionContextConfigurator(a),
                    processMethod.GetTransactionConfig(false),
                    defaults.TransactionContext,
                    typeof(TProcessor).GetTransactionConfig(),
                    TransactionContextAttribute.Default);

                if (config != null)
                    yield return config.CreateSpecification<ConsumerConsumeContext<TProcessor>>();
            }

            if (!messageDeduplicationOverride)
            {
                var config = GetConfig(
                    a => new MessageDeduplicationConfigurator(a),
                    processMethod.GetMessageDeduplicationConfig(false),
                    defaults.MessageDeduplication,
                    typeof(TProcessor).GetMessageDeduplicationConfig(),
                    MessageDeduplicationAttribute.Default);

                if (config != null)
                    yield return config.CreateSpecification<ConsumerConsumeContext<TProcessor>>();
            }

            foreach (var specification in specifications)
                yield return new SpecificationProxy<TProcessor>(specification);
        }

        private TConfig GetConfig<TConfig, TAttrib>(
            Func<TAttrib, TConfig> createConfig,
            TAttrib methodAttribute,
            TConfig typeOverride,
            TAttrib typeOrAssemblyAttrib,
            TAttrib attribDefaults)
            where TConfig : class
            where TAttrib : class, _Attribute
        {
            if (methodAttribute != null)
                return createConfig(methodAttribute);
            else
                return typeOverride ?? createConfig(typeOrAssemblyAttrib ?? attribDefaults);
        }

        class SpecificationProxy<TProcessor> : IPipeSpecification<ConsumerConsumeContext<TProcessor>>
            where TProcessor : class, IMessageProcessor
        {
            private readonly IPipeSpecification<ConsumeContext<TMessage>> _specification;

            public SpecificationProxy(IPipeSpecification<ConsumeContext<TMessage>> specification)
            {
                _specification = specification;
            }

            public void Apply(IPipeBuilder<ConsumerConsumeContext<TProcessor>> builder)
            {
                var messageBuilder = builder as IPipeBuilder<ConsumeContext<TMessage>>;

                if (messageBuilder != null)
                    _specification.Apply(messageBuilder);
            }

            public IEnumerable<ValidationResult> Validate()
            {
                if (!typeof(TProcessor).HasInterface<IMessageProcessor<TMessage>>())
                    yield return this.Failure("MessageType", $"is not consumed by {TypeMetadataCache<TProcessor>.ShortName}");

                foreach (var validationResult in _specification.Validate())
                {
                    yield return validationResult;
                }
            }
        }

        #endregion
    }
}
