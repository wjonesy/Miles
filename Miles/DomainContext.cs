using Miles.Messaging;

namespace Miles
{
    /// <summary>
    /// Convenience type of services commonly needed by any domain process.
    /// </summary>
    public class DomainContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainContext"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="eventPublisher">The event publisher.</param>
        public DomainContext(ITime time, IEventPublisher eventPublisher)
        {
            this.Time = time;
            this.EventPublisher = eventPublisher;
        }

        /// <summary>
        /// Gets the time service.
        /// </summary>
        /// <value>
        /// The time service.
        /// </value>
        public ITime Time { get; private set; }

        /// <summary>
        /// Gets the event publisher service.
        /// </summary>
        /// <value>
        /// The event publisher service.
        /// </value>
        public IEventPublisher EventPublisher { get; private set; }
    }
}
