using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
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
        public async Task Consume(ConsumeContext<ICleanupOutgoingMessages> context)
        {
            await outgoingMessageRepository.DeleteOldDispatchedAsync();
        }
    }
}
