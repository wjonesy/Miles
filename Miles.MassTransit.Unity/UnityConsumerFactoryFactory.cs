using MassTransit;
using Microsoft.Practices.Unity;
using Miles.MassTransit.Configuration;

namespace Miles.MassTransit.Unity
{
    class UnityConsumerFactoryFactory : IConsumerFactoryFactory
    {
        private readonly IUnityContainer container;

        public UnityConsumerFactoryFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public IConsumerFactory<TConsumer> CreateConsumerFactory<TConsumer>() where TConsumer : class
        {
            return new UnityConsumerFactory<TConsumer>(container);
        }
    }
}
