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

namespace Miles.MassTransit
{
    /// <summary>
    /// Interface to the storage mechanism of <see cref="IncomingMessage" />.
    /// </summary>
    /// <remarks>
    /// The idea is this will sit within the transaction of a handler unless otherwise specified.
    /// </remarks>
    public interface IIncomingMessageRepository
    {
        /// <summary>
        /// Records processing the incoming message.
        /// </summary>
        /// <param name="incomingMessage">The incoming message.</param>
        /// <remarks>
        /// Check for duplicates as part of the save. If there is an existing instance we've already processed
        /// the message.
        /// </remarks>
        /// <returns><c>false</c> when the message has not been processed, <c>true</c> if it has already been processed.</returns>
        Task<bool> RecordAsync(IncomingMessage incomingMessage);

        /// <summary>
        /// Deletes the old incoming messages.
        /// </summary>
        /// <returns></returns>
        Task DeleteOldAsync();
    }
}
