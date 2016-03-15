using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miles
{
    public class Hook<TSender, TEventArgs> : IHook<TSender, TEventArgs> where TEventArgs : EventArgs
    {
        private readonly List<Func<TSender, TEventArgs, Task>> hooks = new List<Func<TSender, TEventArgs, Task>>();

        public bool InitiateSynchronously { get; set; } = true;

        public void Register(Func<TSender, TEventArgs, Task> hook)
        {
            hooks.Add(hook);
        }

        public void UnRegister(Func<TSender, TEventArgs, Task> hook)
        {
            hooks.Remove(hook);
        }

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
