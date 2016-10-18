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
using Microsoft.Practices.ServiceLocation;
using Miles.MassTransit.MessageDeduplication;
using Miles.Messaging;
using Miles.Persistence;
using System;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// Configures an <see cref="IMessageProcessor"/> 
    /// </summary>
    /// <typeparam name="TProcessor">The type of the processor.</typeparam>
    /// <seealso cref="global::MassTransit.IPipeConfigurator{global::MassTransit.ConsumerConsumeContext{TProcessor}}" />
    /// <seealso cref="global::MassTransit.ConsumeConfigurators.IConsumeConfigurator" />
    public interface IMessageProcessorConfigurator<TProcessor> : IPipeConfigurator<ConsumerConsumeContext<TProcessor>>, IConsumeConfigurator
        where TProcessor : class, IMessageProcessor
    {
        /// <summary>
        /// Encapsulates the pipe behavior in a <see cref="ITransactionContext" />.
        /// 
        /// Sets defaults for message types processed by <typeparamref name="TProcessor"/>.
        /// </summary>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        IMessageProcessorConfigurator<TProcessor> UseTransactionContext(Action<ITransactionContextConfigurator> configure = null);

        /// <summary>
        /// The message is recorded to ensure it is processed only once.
        /// On identifying a message as already processed the message is removed from the queue without doing any work.
        /// This should be wrapped in an <see cref="ITransactionContext"/> to ensure the processing and recording
        /// of the message are a single unit of work.
        /// 
        /// Sets defaults for message types processed by <typeparamref name="TProcessor"/>.
        /// </summary>
        /// <remarks>
        /// This assumes a container will have registered itself as an <see cref="IServiceLocator"/> payload to 
        /// retrieve an <see cref="IConsumedRepository"/> instance that will work with the <see cref="ITransactionContext"/>.
        /// </remarks>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        IMessageProcessorConfigurator<TProcessor> UseMessageDeduplication(Action<IMessageDeduplicationConfigurator> configure = null);

        /// <summary>
        /// Configure a message type for the processor, such as adding middleware to the pipeline for the message type.
        /// </summary>
        /// <typeparam name="TMessage">The message type</typeparam>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        IMessageProcessorConfigurator<TProcessor> ConfigureMessage<TMessage>(Action<IMessageProcessorMessageConfigurator<TMessage>> configure) where TMessage : class;
    }
}