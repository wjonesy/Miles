using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit.EnsureMessageDispatch
{
    /// <summary>
    /// Use to record dispatch of messages.
    /// <see cref="RecordMessageDispatchFilter{TContext}"/>.
    /// </summary>
    public interface IDispatchedRepository
    {
        /// <summary>
        /// Records a message as being dispatched. This should assume it is operating outside
        /// a transaction (the dispatch has happened, we wouldn't want the record to rollback).
        /// This class should also expect to work as a singleton instance, so think about
        /// thread safety.
        /// </summary>
        /// <param name="context">The message context details.</param>
        /// <returns></returns>
        Task RecordAsync(SendContext context);

        Task DeleteOldRecordsAsync();
    }
}