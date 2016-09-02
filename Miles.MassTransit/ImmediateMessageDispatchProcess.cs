using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Default implementation of <see cref="IMessageDispatchProcess"/> that immediately dispatches the messages.
    /// </summary>
    /// <seealso cref="Miles.MassTransit.IMessageDispatchProcess" />
    public class ImmediateMessageDispatchProcess : IMessageDispatchProcess
    {
        private readonly IOutgoingMessageRepository outgoingMessageRepository;
        private readonly ITime time;
        private readonly IMessageDispatcher commandDispatcher;
        private readonly ConventionBasedMessageDispatcher eventDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImmediateMessageDispatchProcess"/> class.
        /// </summary>
        /// <param name="outgoingMessageRepository">The outgoing message repository.</param>
        /// <param name="time">The time.</param>
        /// <param name="commandDispatcher">The command dispatcher.</param>
        /// <param name="eventDispatcher">The event dispatcher.</param>
        public ImmediateMessageDispatchProcess(
            IOutgoingMessageRepository outgoingMessageRepository,
            ITime time,
            IMessageDispatcher commandDispatcher,
            ConventionBasedMessageDispatcher eventDispatcher)
        {
            this.outgoingMessageRepository = outgoingMessageRepository;
            this.time = time;
            this.commandDispatcher = commandDispatcher;
            this.eventDispatcher = eventDispatcher;
        }

        /// <summary>
        /// Initiates the dispatch of messages to the message queue
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(IEnumerable<OutgoingMessageForDispatch> messages)
        {
            foreach (var message in messages)
            {
                var dispatcher = message.ConceptType == OutgoingMessageConceptType.Command ? commandDispatcher : eventDispatcher;
                try
                {
                    await dispatcher.DispatchAsync(message).ConfigureAwait(false);
                    await outgoingMessageRepository.RecordMessageDispatchAsync(time.Now, message.MessageId).ConfigureAwait(false);
                }
                catch
                {
                    // TODO: Report the failure, but we are intentionally hiding this problem
                }
            }
        }
    }
}
