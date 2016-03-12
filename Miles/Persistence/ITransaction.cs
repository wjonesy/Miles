using System;

namespace Miles.Persistence
{
    public interface ITransaction : IDisposable
    {
        void Begin();

        event EventHandler PreCommit;
        void Commit();
        event EventHandler PostCommit;

        event EventHandler PreRollback;
        void Rollback();
        event EventHandler PostRollback;
    }
}
