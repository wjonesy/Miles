using MassTransit;
using Miles.MassTransit.ConsumerConvention;
using Miles.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class MilesExtensions
    {
        /// <summary>
        /// Registers a Many Miles Message Processor (<see cref="IMessageProcessor{TMessage}" />) with a
        /// MassTransit <see cref="IReceiveEndpointConfigurator" />.
        /// </summary>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="consumerFactoryFactory">The consumer factory factory.</param>
        /// <param name="processorTypes">The processor types.</param>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        public static IReceiveEndpointConfigurator UseMiles(
            this IReceiveEndpointConfigurator configurator,
            IConsumerFactoryFactory consumerFactoryFactory,
            IEnumerable<Type> processorTypes,
            Action<IMilesConfigurator> configure = null)
        {
            var configuration = new MilesConfigurator();
            configure?.Invoke(configuration);

            foreach (var spec in processorTypes.Select(c => configuration.CreateEndpointSpecification(c, consumerFactoryFactory)))
                configurator.AddEndpointSpecification(spec);

            return configurator;
        }
    }
}
