using System.Threading.Tasks;

namespace Miles.MassTransit
{
    public interface IIncomingMessageRepository
    {
        /// <summary>
        /// Records processing the incoming message.
        /// </summary>
        /// <param name="incomingMessage">The incoming message.</param>
        /// <remarks>
        /// Check for duplicates as part of the save. If there is an existing instance we've already processed
        /// the message.
        /// </remarks>
        /// <returns><c>false</c> when the message has not been processed, <c>true</c> if it has already been processed.</returns>
        Task<bool> RecordAsync(IncomingMessage incomingMessage);

        /// <summary>
        /// Deletes the old incoming messages.
        /// </summary>
        /// <returns></returns>
        Task DeleteOldAsync();
    }
}
