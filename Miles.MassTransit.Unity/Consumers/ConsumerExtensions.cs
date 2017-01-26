using MassTransit.ConsumeConfigurators;
using Microsoft.Practices.Unity;
using Miles.MassTransit.Unity;
using System;

namespace MassTransit
{
    public static class ConsumerExtensions
    {
        public static void Consumer<TConsumer>(this IReceiveEndpointConfigurator configurator, IUnityContainer container, Action<IConsumerConfigurator<TConsumer>> configure = null)
            where TConsumer : class, IConsumer
        {
            var consumerFactory = new MilesUnityConsumerFactory<TConsumer>(container);

            configurator.Consumer(consumerFactory, configure);
        }
    }
}
