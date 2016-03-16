using MassTransit;

namespace Miles.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProcessorFactory
    {
        /// <summary>
        /// Creates an event processor.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="consumerContext">The consumer context.</param>
        /// <returns></returns>
        IEventProcessor<TEvent> CreateEventProcessor<TEvent>(ConsumeContext consumerContext);
    }
}
