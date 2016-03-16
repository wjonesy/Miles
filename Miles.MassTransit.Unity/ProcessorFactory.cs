using MassTransit;
using Microsoft.Practices.Unity;

namespace Miles.MassTransit.Unity
{
    /// <summary>
    /// Implemenation of the ProcessorFactory for MassTransit using the Unity container.
    /// </summary>
    /// <seealso cref="Miles.MassTransit.IProcessorFactory" />
    class ProcessorFactory : IProcessorFactory
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
        /// Creates an event processor.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="consumerContext">The consumer context.</param>
        /// <returns></returns>
        public IEventProcessor<TEvent> CreateEventProcessor<TEvent>(ConsumeContext consumerContext)
        {
            return container.Resolve<IEventProcessor<TEvent>>(new ParameterOverride("consumerContext", consumerContext));
        }
    }
}
