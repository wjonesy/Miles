using MassTransit;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Unity
{
    public static class ReceiveEndpointConfiguratorExtensions
    {
        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, IUnityContainer container, params Type[] types)
        {
            configurator.MilesConsumers(container, types);
        }

        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, IUnityContainer container, IEnumerable<Type> types)
        {
            var genericMilesConsumerMethod = typeof(ReceiveEndpointConfiguratorExtensions)
                .GetMethod("MilesConsumer", new[] { typeof(IReceiveEndpointConfigurator), typeof(IUnityContainer) });

            foreach (var type in types)
            {
                var method = genericMilesConsumerMethod.MakeGenericMethod(type);
                method.Invoke(null, new object[] { configurator, container });
            }
        }

        public static void MilesConsumer<TMessage>(this IReceiveEndpointConfigurator configurator, IUnityContainer container) where TMessage : class
        {
            configurator.Consumer(new UnityConsumerFactory<IConsumer<TMessage>>(container));
        }
    }
}
