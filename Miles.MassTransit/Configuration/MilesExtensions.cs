using GreenPipes;
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
        public static void UseMiles<TContext>(this IBusFactoryConfigurator configurator, IConsumerFactoryFactory consumerFactoryFactory, IEnumerable<Type> processorTypes, Action<IMilesConfigurator> configure = null) where TContext : class, PipeContext
        {
            var config = new MilesConfigurator();
            configure?.Invoke(config);

            var receiveEndpointProcessorTypes = processorTypes.Where(x => x.GetCustomAttribute<IgnoreProcessorAttribute>() != null)
                .GroupBy(t => t.GetCustomAttribute<QueueNameAttribute>()?.QueueName ?? t.Name);

            foreach(var endpoint in receiveEndpointProcessorTypes)
            {
                var queueName = endpoint.Key;
                configurator.ReceiveEndpoint(queueName, c =>
                {
                    foreach(var processorType in endpoint)
                    {
                        // TODO: Register processors
                    }
                });
            }
        }
    }
}
