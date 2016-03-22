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
        /// Initializes a new instance of the <see cref="IncomingMessage"/> class.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="messageId">The message identifier.</param>
        public IncomingMessage(string messageType, Guid messageId)
        {
            this.MessageType = messageType;
            this.MessageId = messageId;
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        /// <value>
        /// The type of the message.
        /// </value>
        public string MessageType { get; private set; }

        /// <summary>
        /// Gets the message identifier.
        /// </summary>
        /// <value>
        /// The message identifier.
        /// </value>
        public Guid MessageId { get; private set; }
    }
}
