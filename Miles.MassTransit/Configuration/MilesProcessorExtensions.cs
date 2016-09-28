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
using MassTransit.ConsumeConfigurators;
using Miles.Messaging;
using System;

namespace Miles.MassTransit.Configuration
{
    public static class MilesProcessorExtensions
    {
        public static IReceiveEndpointConfigurator MessageProcessor<TProcessor, TMessage>(
            this IReceiveEndpointConfigurator configurator,
            IConsumerFactory<IConsumer<TMessage>> consumerFactory,
            Action<IConsumerConfigurator<IConsumer<TMessage>>> configure = null,
            bool overrideAttributes = false)
            where TProcessor : class, IMessageProcessor<TMessage>
            where TMessage : class
        {
            configurator.Consumer(consumerFactory, c => ConfigureConsumer<TProcessor, TMessage>(c, configure));
            return configurator;
        }

        public static IReceiveEndpointConfigurator MessageProcessor<TProcessor, TMessage>(
            this IReceiveEndpointConfigurator configurator,
            Func<IConsumer<TMessage>> consumerFactory,
            Action<IConsumerConfigurator<IConsumer<TMessage>>> configure = null,
            bool overrideAttributes = false)
            where TProcessor : class, IMessageProcessor<TMessage>
            where TMessage : class
        {
            configurator.Consumer(consumerFactory, c => ConfigureConsumer<TProcessor, TMessage>(c, configure));
            return configurator;
        }

        public static IReceiveEndpointConfigurator MessageProcessor<TProcessor, TMessage>(
            this IReceiveEndpointConfigurator configurator,
            Action<IConsumerConfigurator<IConsumer<TMessage>>> configure = null,
            bool overrideAttributes = false)
            where TProcessor : class, IMessageProcessor<TMessage>, new()
            where TMessage : class
        {
            configurator.MessageProcessor<TProcessor, TMessage>(() => new ConsumerAdapter<TMessage>(new TProcessor()), configure, overrideAttributes);
            return configurator;
        }

        private static void ConfigureConsumer<TProcessor, TMessage>(IConsumerConfigurator<IConsumer<TMessage>> configurator, Action<IConsumerConfigurator<IConsumer<TMessage>>> configure = null, bool overridingAttributes = false)
            where TProcessor : class, IMessageProcessor<TMessage>
            where TMessage : class
        {
            if (overridingAttributes)
            {
                // TODO: Inspect attributes
                configurator.UseMessageDeduplication();
            }

            configure?.Invoke(configurator);
        }
    }
}
