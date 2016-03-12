using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    public class MassTransitConsumer<TMessage> : IConsumer<TMessage> where TMessage : class
    {
        private readonly IExtendedServiceLocator serviceLocator;

        public MassTransitConsumer(IExtendedServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public Task Consume(ConsumeContext<TMessage> context)
        {
            serviceLocator.RegisterInstance<ConsumeContext>(context);
            var processor = serviceLocator.GetInstance<IEventProcessor<TMessage>>();
            processor.Process(context.Message);
            return Task.FromResult(0);
        }
    }
}
