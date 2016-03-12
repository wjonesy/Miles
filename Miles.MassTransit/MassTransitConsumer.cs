using MassTransit;
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
        private readonly IExtendedServiceLocator serviceLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="MassTransitConsumer{TMessage}"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        public MassTransitConsumer(IExtendedServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        /// <summary>
        /// Processes the incoming message.
        /// </summary>
        /// <param name="context">The consumer context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<TMessage> context)
        {
            serviceLocator.RegisterInstance<ConsumeContext>(context);
            var processor = serviceLocator.GetInstance<IEventProcessor<TMessage>>();
            processor.Process(context.Message);
            return Task.FromResult(0);
        }
    }
}
