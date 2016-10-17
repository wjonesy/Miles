/*
 *     Copyright 2016 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using MassTransit;
using MassTransit.Pipeline.ConsumerFactories;
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
    public static class MilesProcessorExtensions
    {
        #region Individual Processor Registration

        /// <summary>
        /// Registers a Miles Message Processor (<see cref="IMessageProcessor{TMessage}"/>) with a
        /// MassTransit <see cref="IReceiveEndpointConfigurator"/>.
        /// </summary>
        /// <typeparam name="TProcessor">The type of the processor.</typeparam>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="consumerFactory">The consumer factory.</param>
        /// <param name="configure">The consumer configurator applied after Miles configuration.</param>
        /// <param name="ignoreAttributes">if set to <c>true</c> ignores attributes and applies Miles configuration.</param>
        /// <returns></returns>
        public static IReceiveEndpointConfigurator MessageProcessor<TProcessor>(
            this IReceiveEndpointConfigurator configurator,
            IConsumerFactory<TProcessor> consumerFactory,
            Action<IMessageProcessorConfigurator<TProcessor>> configure = null,
            bool ignoreAttributes = false)
            where TProcessor : class, IMessageProcessor
        {
            var processorConfigurator = new MessageProcessorConfigurator<TProcessor>();
            configure?.Invoke(processorConfigurator);

            var specification = processorConfigurator.CreateSpecification(consumerFactory, new MessageProcessorOptions());
            configurator.AddEndpointSpecification(specification);

            return configurator;
        }

        /// <summary>
        /// Registers a Miles Message Processor (<see cref="IMessageProcessor{TMessage}"/>) with a
        /// MassTransit <see cref="IReceiveEndpointConfigurator"/>.
        /// </summary>
        /// <typeparam name="TProcessor">The type of the processor.</typeparam>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="consumerFactory">The consumer factory method.</param>
        /// <param name="configure">The consumer configurator applied after Miles configuration.</param>
        /// <param name="ignoreAttributes">if set to <c>true</c> ignores attributes and applies Miles configuration.</param>
        /// <returns></returns>
        public static IReceiveEndpointConfigurator MessageProcessor<TProcessor>(
            this IReceiveEndpointConfigurator configurator,
            Func<TProcessor> consumerFactory,
            Action<IMessageProcessorConfigurator<TProcessor>> configure = null,
            bool ignoreAttributes = false)
            where TProcessor : class, IMessageProcessor
        {
            return configurator.MessageProcessor(new DelegateConsumerFactory<TProcessor>(consumerFactory), configure, ignoreAttributes);
        }

        /// <summary>
        /// Registers a Miles Message Processor (<see cref="IMessageProcessor{TMessage}"/>) with a
        /// MassTransit <see cref="IReceiveEndpointConfigurator"/>.
        /// </summary>
        /// <typeparam name="TProcessor">The type of the processor.</typeparam>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="configure">The consumer configurator applied after Miles configuration.</param>
        /// <param name="ignoreAttributes">if set to <c>true</c> ignores attributes and applies Miles configuration.</param>
        public static IReceiveEndpointConfigurator MessageProcessor<TProcessor>(
            this IReceiveEndpointConfigurator configurator,
            Action<IMessageProcessorConfigurator<TProcessor>> configure = null,
            bool ignoreAttributes = false)
            where TProcessor : class, IMessageProcessor, new()
        {
            return configurator.MessageProcessor(new DefaultConstructorConsumerFactory<TProcessor>(), configure, ignoreAttributes);
        }

        #endregion

        /// <summary>
        /// Registers a Many Miles Message Processor (<see cref="IMessageProcessor{TMessage}" />) with a
        /// MassTransit <see cref="IReceiveEndpointConfigurator" />.
        /// </summary>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="consumerFactoryFactory">The consumer factory factory.</param>
        /// <param name="processorTypes">The processor types.</param>
        /// <param name="configure">The consumer configurator applied after Miles configuration.</param>
        /// <returns></returns>
        public static IReceiveEndpointConfigurator MessageProcessors(
            this IReceiveEndpointConfigurator configurator,
            IConsumerFactoryFactory consumerFactoryFactory,
            IEnumerable<Type> processorTypes,
            Action<IMessageProcessorsConfigurator> configure = null)
        {
            var configuration = new MessageProcessorsConfigurator();
            configure?.Invoke(configuration);

            foreach (var spec in processorTypes.Select(c => configuration.CreateEndpointSpecification(c, consumerFactoryFactory)))
                configurator.AddEndpointSpecification(spec);

            return configurator;
        }
    }
}
