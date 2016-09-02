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
using System;
using System.Threading.Tasks;

namespace Miles.Messaging
{
    /// <summary>
    /// Represents a service that will publish domain events (or any other events to be honest)
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Registers a handler for immediate execution within a transaction.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="processor">The message processor.</param>
        void Register<TEvent>(IMessageProcessor<TEvent> processor) where TEvent : class;

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="evt">The event.</param>
        void Publish<TEvent>(TEvent evt) where TEvent : class;
    }

    /// <summary>
    /// Convenience helpers for <see cref="IEventPublisher"/> .
    /// </summary>
    public static class EventPublisherExtensions
    {
        /// <summary>
        /// Registers a handler for immediate execution within a transaction.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventPublisher">The event publisher.</param>
        /// <param name="handler">The event handler.</param>
        public static void Register<TEvent>(this IEventPublisher eventPublisher, Func<TEvent, Task> handler) where TEvent : class
        {
            eventPublisher.Register(new CallbackMessageProcessor<TEvent>(handler));
        }
    }
}
