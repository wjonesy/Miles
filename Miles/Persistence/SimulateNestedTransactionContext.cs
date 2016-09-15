/*
 *     Copyright 2016 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Threading.Tasks;

namespace Miles.Persistence
{
    /// <summary>
    /// Base implementation to simulate nested transactions. Inherit and override abstract methods
    /// to perform the transaction commits and rollbacks.
    /// </summary>
    /// <seealso cref="Miles.Persistence.ITransaction" />
    public abstract class SimulateNestedTransactionContext : ITransactionContext, ITransaction
    {
        private readonly Hook<object, EventArgs> preCommitHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> postCommitHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> preRollbackHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> postRollbackHook = new Hook<object, EventArgs>();

        /// <summary>
        /// Hook executed prior to commiting the current transaction.
        /// </summary>
        public IHook<object, EventArgs> PreCommit { get { return preCommitHook; } }

        /// <summary>
        /// Hook executed following commiting the current transaction.
        /// </summary>
        public IHook<object, EventArgs> PostCommit { get { return postCommitHook; } }

        /// <summary>
        /// Hook executed prior to a of the current rollback.
        /// </summary>
        public IHook<object, EventArgs> PreRollback { get { return preRollbackHook; } }

        /// <summary>
        /// Hook executed following a of the current rollback.
        /// </summary>
        public IHook<object, EventArgs> PostRollback { get { return postRollbackHook; } }

        private int nesting = 0;

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<ITransaction> BeginAsync()
        {
            if (nesting == 0)
                await DoBeginAsync().ConfigureAwait(false);
            ++nesting;
            return this;
        }

        /// <summary>
        /// Begins the actual transaction.
        /// </summary>
        /// <returns></returns>
        protected abstract Task DoBeginAsync();

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        /// <returns></returns>
        public async Task CommitAsync()
        {
            // Can't commit without beginning
            if (nesting == 0)
                throw new InvalidOperationException("Attempting to commit a transaction without beginning");

            await preCommitHook.ExecuteAsync(this, new EventArgs()).ConfigureAwait(false);

            // Only commit when on the outter most transaction
            if (nesting == 1)
                await DoCommitAsync().ConfigureAwait(false);
            --nesting;

            await postCommitHook.ExecuteAsync(this, new EventArgs()).ConfigureAwait(false);
        }

        /// <summary>
        /// Initiates the rollback of the transaction.
        /// </summary>
        /// <returns></returns>
        public async Task RollbackAsync()
        {
            // Can't rollback without beginning
            if (nesting == 0)
                throw new InvalidOperationException();

            await preRollbackHook.ExecuteAsync(this, new EventArgs()).ConfigureAwait(false);
            await DoRollbackAsync().ConfigureAwait(false);
            nesting = 0;    // we rollback everything and reset
            await postRollbackHook.ExecuteAsync(this, new EventArgs()).ConfigureAwait(false);
        }

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
        public void Dispose()
        {
            if (nesting > 0)
            {
                var rollback = RollbackAsync();
                if (!rollback.IsCompleted)
                    rollback.RunSynchronously();
            }

            DoDispose();
        }

        /// <summary>
        /// Does the dispose.
        /// </summary>
        protected abstract void DoDispose();
    }
}
