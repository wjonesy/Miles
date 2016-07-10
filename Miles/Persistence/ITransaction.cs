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
        /// Commits the transaction.
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        /// <summary>
        /// Initiates the rollback of the transaction.
        /// </summary>
        /// <returns></returns>
        Task RollbackAsync();
    }
}
