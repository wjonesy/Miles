using System;
using System.Threading.Tasks;

namespace Miles.Persistence
{
    /// <summary>
    /// Represents a database transaction.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        Task BeginAsync();

        /// <summary>
        /// Hook executed prior to commiting the transaction.
        /// </summary>
        /// <value>
        /// The pre commit.
        /// </value>
        IHook<object, EventArgs> PreCommit { get; }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        /// <summary>
        /// Hook executed following commiting the transaction.
        /// </summary>
        /// <value>
        /// The post commit.
        /// </value>
        IHook<object, EventArgs> PostCommit { get; }

        /// <summary>
        /// Hook executed prior to a rollback.
        /// </summary>
        /// <value>
        /// The pre rollback.
        /// </value>
        IHook<object, EventArgs> PreRollback { get; }

        /// <summary>
        /// Initiates the rollback of the transaction.
        /// </summary>
        /// <returns></returns>
        Task RollbackAsync();

        /// <summary>
        /// Hook executed following a rollback.
        /// </summary>
        /// <value>
        /// The post rollback.
        /// </value>
        IHook<object, EventArgs> PostRollback { get; }
    }
}
