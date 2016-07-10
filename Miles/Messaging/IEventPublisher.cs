using System;
using System.Threading.Tasks;

namespace Miles.Messaging
{
    /// <summary>
    /// Represents a service that will publish domain events (or any other events to be honest)
    /// </summary>
    public interface IEventPublisher
    {
        /// <summary>
        /// Registers a handler for immediate execution within a transaction.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="evt">The event.</param>
        void Register<TEvent>(IMessageProcessor<TEvent> evt) where TEvent : class;

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="evt">The event.</param>
        void Publish<TEvent>(TEvent evt) where TEvent : class;
    }

    public static class EventPublisherExtensions
    {
        /// <summary>
        /// Registers a handler for immediate execution within a transaction.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="eventPublisher">The event publisher.</param>
        /// <param name="handler">The event handler.</param>
        public static void Register<TEvent>(this IEventPublisher eventPublisher, Func<TEvent, Task> handler) where TEvent : class
        {
            eventPublisher.Register(new CallbackMessageProcessor<TEvent>(handler));
        }
    }
}
