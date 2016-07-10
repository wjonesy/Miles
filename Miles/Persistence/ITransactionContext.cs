using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Persistence
{
    public interface ITransactionContext
    {
        /// <summary>
        /// Creates a new transaction within the context.
        /// </summary>
        /// <returns></returns>
        Task<ITransaction> BeginAsync();

        /// <summary>
        /// Hook executed prior to commiting the current transaction.
        /// </summary>
        IHook<object, EventArgs> PreCommit { get; }

        /// <summary>
        /// Hook executed following commiting the current transaction.
        /// </summary>
        IHook<object, EventArgs> PostCommit { get; }

        /// <summary>
        /// Hook executed prior to a of the current rollback.
        /// </summary>
        IHook<object, EventArgs> PreRollback { get; }

        /// <summary>
        /// Hook executed following a of the current rollback.
        /// </summary>
        IHook<object, EventArgs> PostRollback { get; }
    }
}
