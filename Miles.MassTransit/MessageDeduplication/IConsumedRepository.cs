using MassTransit;
using Miles.MassTransit.EnsureMessageDispatch;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDeduplication
{
    /// <summary>
    /// Use to record the consumption of messages.
    /// <see cref="RecordMessageDispatchFilter{TContext}"/>.
    /// </summary>
    public interface IConsumedRepository
    {
        /// <summary>
        /// Records a message as consumed.
        /// </summary>
        /// <param name="messageContext">The message context info.</param>
        /// <returns><c>true</c> if the message has already been processed.</returns>
        Task<bool> RecordAsync(MessageContext messageContext);

        /// <summary>
        /// Deletes the old incoming messages.
        /// </summary>
        /// <remarks>
        /// This doesn't need to mean deleting, it could mean archiving. The aim is to keep the data lean for fast processing.
        /// </remarks>
        /// <returns></returns>
        Task DeleteOldRecordsAsync();
    }
}