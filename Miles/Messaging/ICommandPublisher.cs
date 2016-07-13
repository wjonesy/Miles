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
    /// Represents a service that will publish commands.
    /// </summary>
    public interface ICommandPublisher
    {
        /// <summary>
        /// Registers a handler for immediate execution within a transaction.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="cmd">The command.</param>
        void Register<TCommand>(IMessageProcessor<TCommand> cmd) where TCommand : class;

        /// <summary>
        /// Publishes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="cmd">The command.</param>
        void Publish<TCommand>(TCommand cmd) where TCommand : class;
    }

    public static class CommandPublisherExtensions
    {
        /// <summary>
        /// Registers a handler for immediate execution within a transaction.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="commandPublisher">The command publisher.</param>
        /// <param name="handler">The handler.</param>
        public static void Register<TCommand>(this ICommandPublisher commandPublisher, Func<TCommand, Task> handler) where TCommand : class
        {
            commandPublisher.Register(new CallbackMessageProcessor<TCommand>(handler));
        }
    }
}
