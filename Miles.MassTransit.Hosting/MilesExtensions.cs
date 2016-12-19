using MassTransit.Hosting;
using Miles.MassTransit.Configuration;
using Miles.MassTransit.ConsumerConvention;
using Miles.Reflection;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Hosting
{
    public static class MilesExtensions
    {
        /// <summary>
        /// Registers a Many Miles Message Processor (<see cref="IMessageProcessor{TMessage}" />) with a
        /// MassTransit <see cref="IServiceConfigurator" />.
        /// </summary>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="consumerFactoryFactory">The consumer factory factory.</param>
        /// <param name="processorTypes">The processor types.</param>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        public static IServiceConfigurator UseMiles(
            this IServiceConfigurator configurator,
            string queueNamePrefix,
            IConsumerFactoryFactory consumerFactoryFactory,
            IEnumerable<Type> processorTypes,
            Action<IMilesConfigurator> configure = null,
            int? concurrencyLimit = null)
        {
            var configuration = new MilesConfigurator();
            configure?.Invoke(configuration);

            var defaultConcurrencyCount = concurrencyLimit ?? Environment.ProcessorCount;

            foreach (var processor in processorTypes)
            {
                var spec = configuration.CreateEndpointSpecification(processor, consumerFactoryFactory);
                var queueName = processor.GetQueueNameConfig().QueueName;
                configurator.ReceiveEndpoint(queueNamePrefix + "_" + queueName, defaultConcurrencyCount, c => c.AddEndpointSpecification(spec));
            }

            return configurator;
        }
    }
}
