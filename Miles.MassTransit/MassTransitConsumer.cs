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
        private readonly IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitConsumer{TMessage}" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public MassTransitConsumer(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Processes the incoming message.
        /// </summary>
        /// <param name="context">The consumer context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<TMessage> context)
        {
            container.RegisterInstance<ConsumeContext>(context);
            var processor = container.Resolve<IMessageProcessor<TMessage>>();
            return processor.ProcessAsync(context.Message);
        }
    }
}
