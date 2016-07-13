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
using Autofac;
using MassTransit;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Autofac
{
    public static class ReceiveEndpointConfiguratorExtensions
    {
        private const string DefaultName = "message";

        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, ILifetimeScope scope, params Type[] types)
        {
            configurator.MilesConsumers(scope, DefaultName, types);
        }

        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, ILifetimeScope scope, IEnumerable<Type> types)
        {
            configurator.MilesConsumers(scope, DefaultName, types);
        }

        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, ILifetimeScope scope, string name, params Type[] types)
        {
            configurator.MilesConsumers(scope, name, types);
        }

        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, ILifetimeScope scope, string name, IEnumerable<Type> types)
        {
            var genericMilesConsumerMethod = typeof(ReceiveEndpointConfiguratorExtensions)
                .GetMethod("MilesConsumer", new[] { typeof(IReceiveEndpointConfigurator), typeof(ILifetimeScope), typeof(string) });

            foreach (var type in types)
            {
                var method = genericMilesConsumerMethod.MakeGenericMethod(type);
                method.Invoke(null, new object[] { configurator, scope, name });
            }
        }

        public static void MilesConsumer<TMessage>(this IReceiveEndpointConfigurator configurator, ILifetimeScope scope, string name = DefaultName) where TMessage : class
        {
            configurator.Consumer(new AutofacConsumerFactory<IConsumer<TMessage>>(scope, name));
        }
    }
}
