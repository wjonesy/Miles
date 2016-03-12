using System;

namespace Miles.Persistence
{
    public abstract class Transaction : ITransaction
    {
        public abstract void Begin();

        public event EventHandler PreCommit;
        public void Commit()
        {
            if (PreCommit != null)
                PreCommit(this, new EventArgs());

            DoCommit();

            if (PostCommit != null)
                PostCommit(this, new EventArgs());
        }
        public event EventHandler PostCommit;

        protected abstract void DoCommit();

        public event EventHandler PreRollback;
        public void Rollback()
        {
            if (PreRollback != null)
                PreRollback(this, new EventArgs());

            DoRollback();

            if (PostRollback != null)
                PostRollback(this, new EventArgs());
        }

        public event EventHandler PostRollback;

        protected abstract void DoRollback();

        public abstract void Dispose();
    }
}
