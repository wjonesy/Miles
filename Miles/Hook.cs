using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miles
{
    /// <summary>
    /// Similar to the concept of .NET's built in events. The main difference is when handling
    /// async+await is rather than implementing with async void (essentially call and forget) the Task
    /// is returned to allow for normal async handling.
    /// </summary>
    /// <typeparam name="TSender">The type of the sender.</typeparam>
    /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
    /// <seealso cref="Miles.IHook{TSender, TEventArgs}" />
    public class Hook<TSender, TEventArgs> : IHook<TSender, TEventArgs> where TEventArgs : EventArgs
    {
        private readonly List<Func<TSender, TEventArgs, Task>> hooks = new List<Func<TSender, TEventArgs, Task>>();

        /// <summary>
        /// Gets or sets a value indicating whether each hook handler should be executed
        /// one after the other or return a task that waits for each to finish but allows for
        /// simultaneous execution.
        /// </summary>
        /// <value>
        /// <c>true</c> if [initiate synchronously]; otherwise, <c>false</c>.
        /// </value>
        public bool InitiateSynchronously { get; set; } = true;

        public void Register(Func<TSender, TEventArgs, Task> hook)
        {
            hooks.Add(hook);
        }

        public void UnRegister(Func<TSender, TEventArgs, Task> hook)
        {
            hooks.Remove(hook);
        }

        /// <summary>
        /// Calls the registered hooks.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="TEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(TSender sender, TEventArgs args)
        {
            if (InitiateSynchronously)
                foreach (var hook in hooks)
                    await hook(sender, args);
            else
                await Task.WhenAll(hooks.Select(x => x(sender, args)));
        }
    }
}
