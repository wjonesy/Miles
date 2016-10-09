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
using System.Threading.Tasks;

namespace Miles.Messaging
{
    /// <summary>
    /// Indicates the type will implement <see cref="IMessageProcessor{TMessage}"/> 
    /// </summary>
    public interface IMessageProcessor
    { }

    /// <summary>
    /// Processes incoming messages of the specified type.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    public interface IMessageProcessor<in TMessage> : IMessageProcessor
    {
        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task ProcessAsync(TMessage message);
    }
}
