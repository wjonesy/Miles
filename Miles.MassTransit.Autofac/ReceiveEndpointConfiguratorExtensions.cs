using Autofac;
using MassTransit;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Autofac
{
    public static class ReceiveEndpointConfiguratorExtensions
    {
        private const string DefaultName = "message";

        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, ILifetimeScope scope, params Type[] types)
        {
            configurator.MilesConsumers(scope, DefaultName, types);
        }

        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, ILifetimeScope scope, IEnumerable<Type> types)
        {
            configurator.MilesConsumers(scope, DefaultName, types);
        }

        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, ILifetimeScope scope, string name, params Type[] types)
        {
            configurator.MilesConsumers(scope, name, types);
        }

        public static void MilesConsumers(this IReceiveEndpointConfigurator configurator, ILifetimeScope scope, string name, IEnumerable<Type> types)
        {
            var genericMilesConsumerMethod = typeof(ReceiveEndpointConfiguratorExtensions)
                .GetMethod("MilesConsumer", new[] { typeof(IReceiveEndpointConfigurator), typeof(ILifetimeScope), typeof(string) });

            foreach (var type in types)
            {
                var method = genericMilesConsumerMethod.MakeGenericMethod(type);
                method.Invoke(null, new object[] { configurator, scope, name });
            }
        }

        public static void MilesConsumer<TMessage>(this IReceiveEndpointConfigurator configurator, ILifetimeScope scope, string name = DefaultName) where TMessage : class
        {
            configurator.Consumer(new AutofacConsumerFactory<IConsumer<TMessage>>(scope, name));
        }
    }
}
