using MassTransit;
using Miles.Messaging;
using Miles.Persistence;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Sets up the child container/scope such that implementations of Miles interfaces are setup correctly
    /// for MassTransit when resolving the message processor.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <seealso cref="MassTransit.IConsumer{TMessage}" />
    public class DeduplicatedConsumer<TMessage> : IConsumer<TMessage> where TMessage : class
    {
        private readonly static Task CompletedTask = Task.FromResult(0);    // Replace with Task.CompletedTask in .NET 4.6

        private readonly IContainer container;
        private readonly IIncomingMessageRepository incomingMessageRepo;
        private readonly ITransaction transaction;
        private readonly ITime time;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeduplicatedConsumer{TMessage}" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="incomingMessageRepo">The incoming message repo.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="time">The time.</param>
        public DeduplicatedConsumer(IContainer container, IIncomingMessageRepository incomingMessageRepo, ITransaction transaction, ITime time)
        {
            this.container = container;
            this.incomingMessageRepo = incomingMessageRepo;
            this.transaction = transaction;
            this.time = time;
        }

        /// <summary>
        /// Processes the incoming message.
        /// </summary>
        /// <param name="context">The consumer context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<TMessage> context)
        {
            await transaction.BeginAsync();
            try
            {
                // De-duplication
                var incomingMessage = new IncomingMessage(context.MessageId.Value, time.Now);
                var notProcessed = await incomingMessageRepo.SaveAsync(incomingMessage);
                if (!notProcessed)
                {
                    await transaction.RollbackAsync();
                    return;
                }

                container.RegisterInstance<ConsumeContext>(context);
                var processor = container.Resolve<IMessageProcessor<TMessage>>();
                await processor.ProcessAsync(context.Message);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
