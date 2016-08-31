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
using System;
using System.Collections.Generic;

namespace Miles.MassTransit
{
    /// <summary>
    /// Dispatches messages based on the contract type.
    /// </summary>
    /// <remarks>Uses MassTransit's Publish method.</remarks>
    /// <seealso cref="Miles.MassTransit.IMessageDispatcher" />
    public class ConventionBasedMessageDispatcher : IMessageDispatcher
    {
        private readonly IPublishEndpoint publishEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionBasedMessageDispatcher" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="publishEndpoint">The publish endpoint.</param>
        public ConventionBasedMessageDispatcher(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public Task DispatchAsync(IEnumerable<OutgoingMessageForDispatch> ignored)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Dispatches the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageDetails">The message details.</param>
        /// <returns></returns>
        public Task DispatchAsync(object message, OutgoingMessage messageDetails)
        {
            return publishEndpoint.Publish(message, c => { c.MessageId = messageDetails.MessageId; c.CorrelationId = messageDetails.CorrelationId; });
        }
    }
}
