using MassTransit;
using Miles.Messaging;

namespace Miles.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageProcessorFactory
    {
        /// <summary>
        /// Creates a message processor.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="consumerContext">The consumer context.</param>
        /// <returns></returns>
        IMessageProcessor<TMessage> CreateProcessor<TMessage>(ConsumeContext consumerContext);
    }
}
