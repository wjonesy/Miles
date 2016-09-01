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
    /// Represents a message in memory awaiting dispatch.
    /// </summary>
    public class OutgoingMessageForDispatch
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutgoingMessageForDispatch" /> class.
        /// </summary>
        /// <param name="conceptType">Command or Event.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageObject">The message object.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        public OutgoingMessageForDispatch(
            OutgoingMessageConceptType conceptType,
            Type messageType,
            Object messageObject,
            Guid messageId,
            Guid correlationId)
        {
            this.ConceptType = conceptType;
            this.MessageType = messageType;
            this.MessageObject = messageObject;
            this.MessageId = messageId;
            this.CorrelationId = correlationId;
        }

        /// <summary>
        /// Gets a value indicating if the message is a Command or Event.
        /// </summary>
        /// <value>
        /// Command or Event.
        /// </value>
        public OutgoingMessageConceptType ConceptType { get; private set; }

        /// <summary>
        /// Gets the type of the message as indicated by the publish call.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public Type MessageType { get; private set; }

        /// <summary>
        /// Gets the message object.
        /// </summary>
        /// <value>
        /// The message object.
        /// </value>
        public Object MessageObject { get; private set; }

        /// <summary>
        /// Gets the message identifier.
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
    }
}
