using MassTransit;
using Microsoft.Practices.Unity;
using Miles.Events;
using Miles.Persistence;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Unity
{
    public static class UnityContainerExtensions
    {
        public static IUnityContainer LoadMilesMassTransitConfiguration(this IUnityContainer container, IEnumerable<Type> contracts)
        {
            var genericIConsumerType = typeof(IConsumer<>);
            var genericConsumerType = typeof(MassTransitConsumer<>);

            foreach (var contract in contracts)
            {
                var iconsumerContract = genericIConsumerType.MakeGenericType(contract);
                var consumerContract = genericConsumerType.MakeGenericType(contract);
                container.RegisterType(iconsumerContract, consumerContract, new HierarchicalLifetimeManager());
            }

            return container
                .RegisterType<IProcessorFactory, ProcessorFactory>(new HierarchicalLifetimeManager())
                .RegisterType<IEventPublisher, TransactionalMessagePublisher>(
                    new HierarchicalLifetimeManager(),
                    new InjectionConstructor(
                        new ResolvedParameter<ITransaction>(),
                        new ResolvedParameter<IOutgoingMessageRepository>(),
                        new ResolvedParameter<ITime>(),
                        new ResolvedParameter<IBus>(),
                        new OptionalParameter<ConsumeContext>()));
        }

        public static IUnityContainer LoadMassTransitBusConfiguration(this IUnityContainer container, IBusControl busControl)
        {
            return container
                .RegisterInstance<IBusControl>(busControl)
                .RegisterInstance<IBus>(busControl);
        }
    }
}
