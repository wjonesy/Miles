using System;

namespace Miles.MassTransit
{
    /// <summary>
    /// Represents the incoming message serialized for data storage.
    /// </summary>
    public class OutgoingEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutgoingEvent"/> class.
        /// Empty constructor is required by some ORMs and serializers.
        /// </summary>
        protected OutgoingEvent()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutgoingEvent"/> class.
        /// </summary>
        /// <param name="serializedMessage">The serialized message.</param>
        public OutgoingEvent(ITime time, string serializedMessage)
        {
            this.EventCreated = time.Now;
            this.SerializedMessage = serializedMessage;
        }

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
