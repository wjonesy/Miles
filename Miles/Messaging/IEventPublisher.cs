namespace Miles.Messaging
{
    /// <summary>
    /// Represents a service that will publish domain events (or any other events to be honest)
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="evt">The event.</param>
        void Publish<TEvent>(TEvent evt) where TEvent : class;
    }
}
