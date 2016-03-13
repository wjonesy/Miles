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
        /// <param name="transactionMessageId">The transaction message identifier.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="serializedMessage">The serialized message.</param>
        /// <param name="time">Time service.</param>
        public OutgoingMessage(Guid transactionMessageId, OutgoingMessageType messageType, string serializedMessage, ITime time)
        {
            this.EventCreated = time.Now;
            this.TransactionMessageId = transactionMessageId;
            this.MessageType = messageType;
            this.SerializedMessage = serializedMessage;
        }

        /// <summary>
        /// Gets a unique identifier used for message de-deuplication between Miles endpoints.
        /// </summary>
        /// <value>
        /// The transaction message identifier.
        /// </value>
        public Guid TransactionMessageId { get; private set; }

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
        public void Dispatched(ITime time)
        {
            EventDispatched = time.Now;
        }
    }
}
