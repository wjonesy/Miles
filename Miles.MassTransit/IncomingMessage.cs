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
        /// The when.
        /// </value>
        public DateTime When { get; private set; }
    }
}
