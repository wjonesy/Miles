using System;

namespace Miles.MassTransit.EntityFramework.MessageDeduplication
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
        public IncomingMessage(Guid messageId, string queueName, DateTime when)
        {
            this.MessageId = messageId;
            this.QueueName = queueName;
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
        /// Gets the name of the queue.
        /// </summary>
        /// <value>
        /// The name of the queue.
        /// </value>
        public string QueueName { get; private set; }

        /// <summary>
        /// Gets when the message was processed.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime When { get; private set; }
    }
}
