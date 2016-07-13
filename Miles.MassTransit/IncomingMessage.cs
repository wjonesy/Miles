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

namespace Miles.MassTransit
{
    /// <summary>
    /// Record of incoming messages to avoid processing duplicates.
    /// </summary>
    public class IncomingMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncomingMessage"/> class.
        /// </summary>
        protected IncomingMessage()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomingMessage" /> class.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="when">When the message was processed.</param>
        public IncomingMessage(Guid messageId, DateTime when)
        {
            this.MessageId = messageId;
            this.When = when;
        }

        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public Guid MessageId { get; private set; }

        /// <summary>
        /// Gets when the message was processed.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime When { get; private set; }
    }
}
