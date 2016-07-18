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
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Unity
{
    /// <exclude />
    public static class ReceiveEndpointConfiguratorExtensions
    {
        /// <summary>
        /// Configures the receive endpoint to consume the supplied message types retrieving the consumers from unity.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="container">The container.</param>
        /// <param name="types">The message types.</param>
        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, IUnityContainer container, params Type[] types)
        {
            configurator.MilesConsumers(container, types);
        }

        /// <summary>
        /// Configures the receive endpoint to consume the supplied message types retrieving the consumers from unity.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="container">The container.</param>
        /// <param name="types">The message types.</param>
        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, IUnityContainer container, IEnumerable<Type> types)
        {
            var genericMilesConsumerMethod = typeof(ReceiveEndpointConfiguratorExtensions)
                .GetMethod("MilesConsumer", new[] { typeof(IReceiveEndpointConfigurator), typeof(IUnityContainer) });

            foreach (var type in types)
            {
                var method = genericMilesConsumerMethod.MakeGenericMethod(type);
                method.Invoke(null, new object[] { configurator, container });
            }
        }

        /// <summary>
        /// Configures the receive endpoint to consume the supplied message type retrieving the consumer from unity.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <param name="container">The container.</param>
        public static void MilesConsumer<TMessage>(this IReceiveEndpointConfigurator configurator, IUnityContainer container) where TMessage : class
        {
            configurator.Consumer(new UnityConsumerFactory<IConsumer<TMessage>>(container));
        }
    }
}
