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
    public static class MilesExtensions
    {
        /// <summary>
        /// Registers a Many Miles Message Processor (<see cref="IMessageProcessor{TMessage}" />) with a
        /// MassTransit <see cref="IReceiveEndpointConfigurator" />.
        /// </summary>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="container">Unity container that will create the processors.</param>
        /// <param name="processorTypes">The processor types.</param>
        /// <param name="configure">The consumer configurator applied after Miles configuration.</param>
        /// <returns></returns>
        public static IReceiveEndpointConfigurator UseMiles(
            this IReceiveEndpointConfigurator configurator,
            IUnityContainer container,
            IEnumerable<Type> processorTypes,
            Action<IMilesConfigurator> configure = null)
        {
            return configurator.UseMiles(new UnityConsumerFactoryFactory(container), processorTypes, configure);
        }
    }
}
