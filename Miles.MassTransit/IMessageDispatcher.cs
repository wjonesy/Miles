using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Dispatches a message.
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// Dispatches the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageDetails">The message details.</param>
        /// <returns></returns>
        Task DispatchAsync(object message, OutgoingMessage messageDetails);
    }
}
