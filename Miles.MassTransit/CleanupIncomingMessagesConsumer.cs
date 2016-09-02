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
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// MassTransit consumer that cleans up old incoming messages.
    /// </summary>
    /// <seealso cref="IConsumer{ICleanupIncomingMessages}" />
    public class CleanupIncomingMessagesConsumer : IConsumer<ICleanupIncomingMessagesCommand>
    {
        private readonly IIncomingMessageRepository incomingMessageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupIncomingMessagesConsumer"/> class.
        /// </summary>
        /// <param name="incomingMessageRepository">The incoming message repository.</param>

        public CleanupIncomingMessagesConsumer(IIncomingMessageRepository incomingMessageRepository)
        {
            this.incomingMessageRepository = incomingMessageRepository;
        }

        /// <summary>
        /// Consumes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<ICleanupIncomingMessagesCommand> context)
        {
            return incomingMessageRepository.DeleteOldAsync();
        }
    }
}
