using System;
using System.Threading.Tasks;

namespace Miles.Messaging
{
    class CallbackMessageProcessor<TEvent> : IMessageProcessor<TEvent> where TEvent : class
    {
        private readonly Func<TEvent, Task> callback;

        public CallbackMessageProcessor(Func<TEvent, Task> callback)
        {
            this.callback = callback;
        }

        public Task ProcessAsync(TEvent message)
        {
            return callback(message);
        }
    }
}
