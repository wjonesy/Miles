using System;
using System.Threading.Tasks;

namespace Miles
{
    public interface IHook<TSender, TEventArgs> where TEventArgs : EventArgs
    {
        void Register(Func<TSender, TEventArgs, Task> hook);
        void UnRegister(Func<TSender, TEventArgs, Task> hook);
    }
}
