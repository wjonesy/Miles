using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    public class OutOfThreadMessageDispatchProcess : IMessageDispatchProcess
    {
        private static readonly Task AlreadyCompleted = Task.FromResult(0);

        private readonly BlockingCollection<OutgoingMessageForDispatch> queue = new BlockingCollection<OutgoingMessageForDispatch>(new ConcurrentQueue<OutgoingMessageForDispatch>());

        private readonly ConventionBasedMessageDispatcher eventDispatcher;
        private readonly IMessageDispatcher commandDispatcher;

        public OutOfThreadMessageDispatchProcess()
        {
            var t = Task.Run(async () =>
            {
                var message = queue.Take();
                while (message != null)
                {
                    var dispatcher = message.ConceptType == OutgoingMessageConceptType.Command ? commandDispatcher : eventDispatcher;
                    await dispatcher.DispatchAsync(message);

                    message = queue.Take();
                }
            });
        }

        public Task ExecuteAsync(IEnumerable<OutgoingMessageForDispatch> messages)
        {
            foreach (var message in messages)
                queue.Add(message);

            return AlreadyCompleted;
        }
    }
}
