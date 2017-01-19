using MassTransit;
using Miles.MassTransit.ConsumerConvention;
using Miles.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Miles.MassTransit.Configuration
{
    public static class MilesExtensions
    {
        public static void UseMiles(this IBusFactoryConfigurator configurator, IConsumerFactoryFactory consumerFactoryFactory, IEnumerable<Type> processorTypes, Action<IMilesConfigurator> configure = null)
        {
            var config = new MilesConfigurator();
            configure?.Invoke(config);

            var receiveEndpointProcessorTypes = processorTypes.Where(x => x.GetCustomAttribute<IgnoreProcessorAttribute>() == null)
                .GroupBy(t => t.GetCustomAttribute<QueueNameAttribute>()?.QueueName ?? t.Name);

            var configureMessageProcessor = typeof(MilesExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod)
                .Where(x => x.Name == "ConfigureMessageProcessorUsingAttributes")
                .Where(x => x.IsGenericMethodDefinition)
                .Single();

            foreach(var endpoint in receiveEndpointProcessorTypes)
            {
                var queueName = !string.IsNullOrWhiteSpace(config.QueueNamePrefix) ? config.QueueNamePrefix + "_" + endpoint.Key : endpoint.Key;
                configurator.ReceiveEndpoint(queueName, c =>
                {
                    foreach (var processorType in endpoint)
                        configureMessageProcessor.MakeGenericMethod(processorType).Invoke(null, new object[] { c, consumerFactoryFactory });
                });
            }
        }

        private static void ConfigureMessageProcessorUsingAttributes<TProcessor>(this IReceiveEndpointConfigurator configurator, IConsumerFactoryFactory consumerFactoryFactory)
            where TProcessor : class, IMessageProcessor
        {
            configurator.MessageProcessor<TProcessor>(consumerFactoryFactory.CreateConsumerFactory<TProcessor>());
        }
    }
}
