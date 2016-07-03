using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// MassTransit consumer that cleans up old outgoing messages.
    /// </summary>
    /// <seealso cref="MassTransit.IConsumer{Miles.MassTransit.ICleanupOutgoingMessages}" />
    public class CleanupOutgoingMessagesConsumer : IConsumer<ICleanupOutgoingMessages>
    {
        private readonly IOutgoingMessageRepository outgoingMessageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupIncomingMessagesConsumer"/> class.
        /// </summary>
        /// <param name="incomingMessageRepository">The incoming message repository.</param>

        public CleanupOutgoingMessagesConsumer(IOutgoingMessageRepository outgoingMessageRepository)
        {
            this.outgoingMessageRepository = outgoingMessageRepository;
        }

        /// <summary>
        /// Consumes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<ICleanupOutgoingMessages> context)
        {
            return outgoingMessageRepository.DeleteOldDispatchedAsync();
        }
    }
}
