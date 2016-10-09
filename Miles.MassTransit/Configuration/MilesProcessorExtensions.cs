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
            var processorConfigurator = new MessageProcessorConfigurator<TProcessor>(consumerFactory);

            // ignore attributes or not
            if (!ignoreAttributes)
            {
                // first apply the transaction if desired to wrap the message deduplication handling
                // In case someone applied the attrib at a method level start as ProcessAsync and work backwards
                //var transactionContextAttribute = typeof(TProcessor).GetMethod("ProcessAsync").GetTransactionConfig();
                var transactionContextAttribute = typeof(TProcessor).GetTransactionConfig();
                if (transactionContextAttribute.Enabled)
                    processorConfigurator.UseTransactionContext(c => c.HintIsolationLevel(transactionContextAttribute.HintIsolationLevel));

                if (typeof(TProcessor).IsMessageDeduplicationEnabled())
                    processorConfigurator.UseMessageDeduplication();
            }

            configure?.Invoke(processorConfigurator);

            configurator.AddEndpointSpecification(processorConfigurator);

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
    }
}
