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
    /// Represents the outgoing message serialized for data storage.
    /// </summary>
    public class OutgoingMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutgoingMessage"/> class.
        /// Empty constructor is required by some ORMs and serializers.
        /// </summary>
        protected OutgoingMessage()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutgoingMessage" /> class.
        /// </summary>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="serializedMessage">The serialized message.</param>
        /// <param name="eventCreated">Time service.</param>
        public OutgoingMessage(Guid messageId, Guid correlationId, OutgoingMessageType messageType, string serializedMessage, DateTime eventCreated)
        {
            this.EventCreated = eventCreated;
            this.MessageId = messageId;
            this.CorrelationId = correlationId;
            this.MessageType = messageType;
            this.SerializedMessage = serializedMessage;
        }

        /// <summary>
        /// Gets a unique identifier used for message de-duplication between endpoints.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public Guid MessageId { get; private set; }

        /// <summary>
        /// Gets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        public Guid CorrelationId { get; private set; }

        /// <summary>
        /// Gets the message type.
        /// </summary>
        /// <value>
        /// The message type.
        /// </value>
        public OutgoingMessageType MessageType { get; private set; }

        /// <summary>
        /// Gets the serialized message.
        /// </summary>
        /// <value>
        /// The serialized message.
        /// </value>
        public string SerializedMessage { get; private set; }

        /// <summary>
        /// Gets when the event was created.
        /// </summary>
        /// <value>
        /// When the event was created.
        /// </value>
        public DateTime EventCreated { get; private set; }

        /// <summary>
        /// Gets when the event was dispatched. If <c>null</c> then the message has not yet been dispatched.
        /// </summary>
        /// <value>
        /// When the event was dispatched.
        /// </value>
        public DateTime? EventDispatched { get; private set; }

        /// <summary>
        /// Indicate the message has been dispatched.
        /// </summary>
        /// <param name="time">The time.</param>
        public void Dispatched(DateTime time)
        {
            EventDispatched = time;
        }
    }
}
