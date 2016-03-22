using System.Threading.Tasks;

namespace Miles.MassTransit
{
    public interface IIncomingMessageRepository
    {
        /// <summary>
        /// Saves the instance.
        /// </summary>
        /// <param name="incomingMessage">The incoming message.</param>
        /// <remarks>
        /// Check for duplicates as part of the save. If there is an existing instance we've already processed
        /// the message.
        /// </remarks>
        /// <returns><c>true</c> when the instance is unique, <c>false</c> on primary key clash.</returns>
        Task<bool> SaveAsync(IncomingMessage incomingMessage);
    }
}
