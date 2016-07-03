using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// MassTransit consumer that cleans up old incoming messages.
    /// </summary>
    /// <seealso cref="MassTransit.IConsumer{Miles.MassTransit.ICleanupIncomingMessages}" />
    public class CleanupIncomingMessagesConsumer : IConsumer<ICleanupIncomingMessages>
    {
        private readonly IIncomingMessageRepository incomingMessageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CleanupIncomingMessagesConsumer"/> class.
        /// </summary>
        /// <param name="incomingMessageRepository">The incoming message repository.</param>

        public CleanupIncomingMessagesConsumer(IIncomingMessageRepository incomingMessageRepository)
        {
            this.incomingMessageRepository = incomingMessageRepository;
        }

        /// <summary>
        /// Consumes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<ICleanupIncomingMessages> context)
        {
            return incomingMessageRepository.DeleteOldAsync();
        }
    }
}
