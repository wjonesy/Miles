﻿/*
 *     Copyright 2017 Adam Burton (adz21c@gmail.com)
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
using MassTransit.ConsumeConfigurators;
using Microsoft.Practices.Unity;
using Miles.MassTransit.Unity;
using System;

namespace MassTransit
{
    public static class ConsumerExtensions
    {
        public static void Consumer<TConsumer>(this IReceiveEndpointConfigurator configurator, IUnityContainer container, Action<IConsumerConfigurator<TConsumer>> configure = null)
            where TConsumer : class, IConsumer
        {
            var consumerFactory = new MilesUnityConsumerFactory<TConsumer>(container);

            configurator.Consumer(consumerFactory, configure);
        }
    }
}
