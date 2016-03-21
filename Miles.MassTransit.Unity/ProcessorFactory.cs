using MassTransit;
using Microsoft.Practices.Unity;
using Miles.Messaging;

namespace Miles.MassTransit.Unity
{
    /// <summary>
    /// Implemenation of the ProcessorFactory for MassTransit using the Unity container.
    /// </summary>
    /// <seealso cref="Miles.MassTransit.IMessageProcessorFactory" />
    class ProcessorFactory : IMessageProcessorFactory
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public ProcessorFactory(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Creates the processor.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="consumerContext">The consumer context.</param>
        /// <returns></returns>
        public IMessageProcessor<TEvent> CreateProcessor<TEvent>(ConsumeContext consumerContext)
        {
            return container.Resolve<IMessageProcessor<TEvent>>(new ParameterOverride("consumerContext", consumerContext));
        }
    }
}
