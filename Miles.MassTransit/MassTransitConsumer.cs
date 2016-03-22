using MassTransit;
using Miles.Messaging;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Sets up the child container/scope such that implementations of Miles interfaces are setup correctly
    /// for MassTransit when resolving the message processor.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <seealso cref="MassTransit.IConsumer{TMessage}" />
    public class MassTransitConsumer<TMessage> : IConsumer<TMessage> where TMessage : class
    {
        private readonly static Task CompletedTask = Task.FromResult(0);    // Replace with Task.CompletedTask in .NET 4.6
        private readonly IContainer container;
        private readonly IIncomingMessageRepository incomingMessageRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitConsumer{TMessage}" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public MassTransitConsumer(IContainer container, IIncomingMessageRepository incomingMessageRepo)
        {
            this.container = container;
            this.incomingMessageRepo = incomingMessageRepo;
        }

        /// <summary>
        /// Processes the incoming message.
        /// </summary>
        /// <param name="context">The consumer context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<TMessage> context)
        {
            // De-duplication
            var incomingMessage = new IncomingMessage(typeof(TMessage).FullName, context.MessageId.Value);
            var notProcessed = await incomingMessageRepo.SaveAsync(incomingMessage);
            if (!notProcessed)
                await CompletedTask;

            container.RegisterInstance<ConsumeContext>(context);
            var processor = container.Resolve<IMessageProcessor<TMessage>>();
            await processor.ProcessAsync(context.Message);
        }
    }
}
