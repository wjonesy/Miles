using System.Threading.Tasks;

namespace Miles.Messaging
{
    /// <summary>
    /// Processes incoming messages of the specified type.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    public interface IMessageProcessor<in TMessage>
    {
        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task ProcessAsync(TMessage message);
    }
}
