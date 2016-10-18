using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDispatch
{
    /// <summary>
    /// Dispatches messages in a separate thread to allow processing code to complete earlier.
    /// </summary>
    /// <seealso cref="IMessageDispatchProcess" />
    public class OutOfThreadMessageDispatchProcess : IMessageDispatchProcess
    {
        private static readonly Task AlreadyCompleted = Task.FromResult(0);

        private readonly BlockingCollection<OutgoingMessageForDispatch> queue = new BlockingCollection<OutgoingMessageForDispatch>(new ConcurrentQueue<OutgoingMessageForDispatch>());

        private readonly ConventionBasedMessageDispatcher eventDispatcher;
        private readonly IMessageDispatcher commandDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfThreadMessageDispatchProcess"/> class.
        /// </summary>
        /// <param name="eventDispatcher">The event dispatcher.</param>
        /// <param name="commandDispatcher">The command dispatcher.</param>
        public OutOfThreadMessageDispatchProcess(
            ConventionBasedMessageDispatcher eventDispatcher,
            IMessageDispatcher commandDispatcher)
        {
            this.eventDispatcher = eventDispatcher;
            this.commandDispatcher = commandDispatcher;

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

        /// <summary>
        /// Initiates the dispatch of messages to the message queue
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public Task ExecuteAsync(IEnumerable<OutgoingMessageForDispatch> messages)
        {
            foreach (var message in messages)
                queue.Add(message);

            return AlreadyCompleted;
        }
    }
}
