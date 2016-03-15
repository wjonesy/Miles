using System;
using System.Threading.Tasks;

namespace Miles.Persistence
{
    public interface ITransaction : IDisposable
    {
        Task BeginAsync();

        IHook<object, EventArgs> PreCommit { get; }
        Task CommitAsync();
        IHook<object, EventArgs> PostCommit { get; }

        IHook<object, EventArgs> PreRollback { get; }
        Task RollbackAsync();
        IHook<object, EventArgs> PostRollback { get; }
    }
}
