using MassTransit;
using Microsoft.Practices.Unity;

namespace Miles.MassTransit.Unity
{
    class ProcessorFactory : IProcessorFactory
    {
        private readonly IUnityContainer container;

        public ProcessorFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public IEventProcessor<TEvent> CreateEventProcessor<TEvent>(ConsumeContext consumerContext)
        {
            return container.Resolve<IEventProcessor<TEvent>>(new ParameterOverride("consumerContext", consumerContext));
        }
    }
}
