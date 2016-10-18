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
