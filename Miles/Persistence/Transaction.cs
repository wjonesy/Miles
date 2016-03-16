using System;
using System.Threading.Tasks;

namespace Miles.Persistence
{
    /// <summary>
    /// Abstract base implementation that executes the hooks when necessary.
    /// </summary>
    /// <seealso cref="Miles.Persistence.ITransaction" />
    public abstract class Transaction : ITransaction
    {
        private readonly Hook<object, EventArgs> preCommitHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> postCommitHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> preRollbackHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> postRollbackHook = new Hook<object, EventArgs>();

        /// <summary>
        /// Hook executed prior to commiting the transaction.
        /// </summary>
        /// <value>
        /// The pre commit.
        /// </value>
        public IHook<object, EventArgs> PreCommit { get { return preCommitHook; } }
        /// <summary>
        /// Hook executed following commiting the transaction.
        /// </summary>
        /// <value>
        /// The post commit.
        /// </value>
        public IHook<object, EventArgs> PostCommit { get { return postCommitHook; } }
        /// <summary>
        /// Hook executed prior to a rollback.
        /// </summary>
        /// <value>
        /// The pre rollback.
        /// </value>
        public IHook<object, EventArgs> PreRollback { get { return preRollbackHook; } }
        /// <summary>
        /// Hook executed following a rollback.
        /// </summary>
        /// <value>
        /// The post rollback.
        /// </value>
        public IHook<object, EventArgs> PostRollback { get { return postRollbackHook; } }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            await preCommitHook.ExecuteAsync(this, new EventArgs());
            await DoCommitAsync();
            await postCommitHook.ExecuteAsync(this, new EventArgs());
        }

        /// <summary>
        /// Initiates the rollback of the transaction.
        /// </summary>
        /// <returns></returns>
        public async Task RollbackAsync()
        {
            await preRollbackHook.ExecuteAsync(this, new EventArgs());
            await DoRollbackAsync();
            await postRollbackHook.ExecuteAsync(this, new EventArgs());
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        public abstract Task BeginAsync();

        /// <summary>
        /// Override to perform the actual commit.
        /// </summary>
        /// <returns></returns>
        protected abstract Task DoCommitAsync();

        /// <summary>
        /// Override to perform the actual rollback.
        /// </summary>
        /// <returns></returns>
        protected abstract Task DoRollbackAsync();

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();
    }
}
