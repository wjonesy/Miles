using System;
using System.Threading.Tasks;

namespace Miles.Persistence
{
    public abstract class Transaction : ITransaction
    {
        private readonly Hook<object, EventArgs> preCommitHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> postCommitHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> preRollbackHook = new Hook<object, EventArgs>();
        private readonly Hook<object, EventArgs> postRollbackHook = new Hook<object, EventArgs>();

        public IHook<object, EventArgs> PreCommit { get { return preCommitHook; } }
        public IHook<object, EventArgs> PostCommit { get { return postCommitHook; } }
        public IHook<object, EventArgs> PreRollback { get { return preRollbackHook; } }
        public IHook<object, EventArgs> PostRollback { get { return postRollbackHook; } }

        public async Task CommitAsync()
        {
            await preCommitHook.ExecuteAsync(this, new EventArgs());
            await DoCommitAsync();
            await postCommitHook.ExecuteAsync(this, new EventArgs());
        }

        public async Task RollbackAsync()
        {
            await preRollbackHook.ExecuteAsync(this, new EventArgs());
            await DoRollbackAsync();
            await postRollbackHook.ExecuteAsync(this, new EventArgs());
        }

        public abstract Task BeginAsync();

        protected abstract Task DoCommitAsync();

        protected abstract Task DoRollbackAsync();

        public abstract void Dispose();
    }
}
