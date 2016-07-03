using System;
using System.Threading.Tasks;

namespace Miles
{
    /// <summary>
    /// Subset of <see cref="Hook"/>'s public interface that exposes only registration to the hook.
    /// The idea is to make the interface public and only use the full implementation within a class
    /// to prevent calling code executing hooks.
    /// </summary>
    /// <typeparam name="TSender">The type of the sender.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    public interface IHook<TSender, TEventArgs> where TEventArgs : EventArgs
    {
        /// <summary>
        /// Registers the specified hook.
        /// </summary>
        /// <param name="hook">The hook.</param>
        void Register(Func<TSender, TEventArgs, Task> hook);

        /// <summary>
        /// Unregisters the specified hook.
        /// </summary>
        /// <param name="hook">The hook.</param>
        void UnRegister(Func<TSender, TEventArgs, Task> hook);
    }
}
