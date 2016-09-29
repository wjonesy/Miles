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
using Miles.Reflection;
using System;

namespace Miles.MassTransit.Configuration
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
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="consumerFactory">The consumer factory.</param>
        /// <param name="configure">The consumer configurator applied after Miles configuration.</param>
        /// <param name="ignoreAttributes">if set to <c>true</c> ignores attributes and applies Miles configuration.</param>
        /// <returns></returns>
        public static IReceiveEndpointConfigurator MessageProcessor<TProcessor, TMessage>(
            this IReceiveEndpointConfigurator configurator,
            IConsumerFactory<IConsumer<TMessage>> consumerFactory,
            Action<IConsumerConfigurator<IConsumer<TMessage>>> configure = null,
            bool ignoreAttributes = false)
            where TProcessor : class, IMessageProcessor<TMessage>
            where TMessage : class
        {
            configurator.Consumer(consumerFactory, c => ConfigureConsumer<TProcessor, TMessage>(c, configure));
            return configurator;
        }

        /// <summary>
        /// Registers a Miles Message Processor (<see cref="IMessageProcessor{TMessage}"/>) with a
        /// MassTransit <see cref="IReceiveEndpointConfigurator"/>.
        /// </summary>
        /// <typeparam name="TProcessor">The type of the processor.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="consumerFactory">The consumer factory method.</param>
        /// <param name="configure">The consumer configurator applied after Miles configuration.</param>
        /// <param name="ignoreAttributes">if set to <c>true</c> ignores attributes and applies Miles configuration.</param>
        /// <returns></returns>
        public static IReceiveEndpointConfigurator MessageProcessor<TProcessor, TMessage>(
            this IReceiveEndpointConfigurator configurator,
            Func<IConsumer<TMessage>> consumerFactory,
            Action<IConsumerConfigurator<IConsumer<TMessage>>> configure = null,
            bool ignoreAttributes = false)
            where TProcessor : class, IMessageProcessor<TMessage>
            where TMessage : class
        {
            configurator.Consumer(consumerFactory, c => ConfigureConsumer<TProcessor, TMessage>(c, configure));
            return configurator;
        }

        /// <summary>
        /// Registers a Miles Message Processor (<see cref="IMessageProcessor{TMessage}"/>) with a
        /// MassTransit <see cref="IReceiveEndpointConfigurator"/>.
        /// </summary>
        /// <typeparam name="TProcessor">The type of the processor.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="configurator">The receive endpoint configurator.</param>
        /// <param name="configure">The consumer configurator applied after Miles configuration.</param>
        /// <param name="ignoreAttributes">if set to <c>true</c> ignores attributes and applies Miles configuration.</param>
        public static IReceiveEndpointConfigurator MessageProcessor<TProcessor, TMessage>(
            this IReceiveEndpointConfigurator configurator,
            Action<IConsumerConfigurator<IConsumer<TMessage>>> configure = null,
            bool ignoreAttributes = false)
            where TProcessor : class, IMessageProcessor<TMessage>, new()
            where TMessage : class
        {
            configurator.MessageProcessor<TProcessor, TMessage>(() => new ConsumerAdapter<TMessage>(new TProcessor()), configure, ignoreAttributes);
            return configurator;
        }

        private static void ConfigureConsumer<TProcessor, TMessage>(IConsumerConfigurator<IConsumer<TMessage>> configurator, Action<IConsumerConfigurator<IConsumer<TMessage>>> configure = null, bool ignoreAttributes = false)
            where TProcessor : class, IMessageProcessor<TMessage>
            where TMessage : class
        {
            // ignore attributes or not
            if (!ignoreAttributes)
            {
                // first apply the transaction if desired to wrap the message deduplication handling
                // In case someone applied the attrib at a method level start as ProcessAsync and work backwards
                var transactionContextAttribute = typeof(TProcessor).GetMethod("ProcessAsync").GetTransactionConfig();
                if (transactionContextAttribute.Enabled)
                    configurator.UseTransactionContext(c => c.HintIsolationLevel(transactionContextAttribute.HintIsolationLevel));

                if (typeof(TProcessor).IsMessageDeduplicationEnabled())
                    configurator.UseMessageDeduplication();
            }

            configure?.Invoke(configurator);
        }
    }
}
