using MassTransit;
using Microsoft.Practices.Unity;
using Miles.MassTransit.Configuration;
using Miles.Messaging;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Unity
{
    /// <summary>
    /// 
    /// </summary>
    public static class MilesProcessorExtensions
    {
        /// <summary>
        /// Registers a Miles Message Processor (<see cref="IMessageProcessor{TMessage}"/>) with a
        /// MassTransit <see cref="IReceiveEndpointConfigurator"/>.
        /// </summary>
        /// <typeparam name="TProcessor">The type of the processor.</typeparam>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="container">The container that will create the processor.</param>
        /// <param name="configure">The consumer configurator applied after Miles configuration.</param>
        /// <param name="ignoreAttributes">if set to <c>true</c> ignores attributes and applies Miles configuration.</param>
        /// <returns></returns>
        public static IReceiveEndpointConfigurator MessageProcessor<TProcessor>(
            this IReceiveEndpointConfigurator configurator,
            IUnityContainer container,
            Action<IMessageProcessorConfigurator<TProcessor>> configure = null,
            bool ignoreAttributes = false)
            where TProcessor : class, IMessageProcessor
        {
            return configurator.MessageProcessor(new UnityConsumerFactory<TProcessor>(container), configure);
        }

        /// <summary>
        /// Registers a Many Miles Message Processor (<see cref="IMessageProcessor{TMessage}" />) with a
        /// MassTransit <see cref="IReceiveEndpointConfigurator" />.
        /// </summary>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="container">Unity container that will create the processors.</param>
        /// <param name="processorTypes">The processor types.</param>
        /// <param name="configure">The consumer configurator applied after Miles configuration.</param>
        /// <returns></returns>
        public static IReceiveEndpointConfigurator MessageProcessors(
            this IReceiveEndpointConfigurator configurator,
            IUnityContainer container,
            IEnumerable<Type> processorTypes,
            Action<IMessageProcessorsConfigurator> configure = null)
        {
            return configurator.MessageProcessors(new UnityConsumerFactoryFactory(container), processorTypes, configure);
        }
    }
}
