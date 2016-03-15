using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Interface to the storage mechanism of <see cref="OutgoingMessage" />.
    /// </summary>
    /// <remarks>
    /// The idea is this will sit within the transaction of a handler.
    /// </remarks>
    public interface IOutgoingMessageRepository
    {
        /// <summary>
        /// Saves the specified events to the storage mechanism.
        /// </summary>
        /// <param name="evts">The events.</param>
        Task SaveAsync(IEnumerable<OutgoingMessage> evts);

        /// <summary>
        /// Saves the specified event to the storage mechanism.
        /// </summary>
        /// <param name="evt">The event.</param>
        /// <param name="ignoreTransaction">if set to <c>true</c> then the save shouldn't be part of a transaction; this change should save in isolation.</param>
        Task SaveAsync(OutgoingMessage evt, bool ignoreTransaction = false);
    }
}
