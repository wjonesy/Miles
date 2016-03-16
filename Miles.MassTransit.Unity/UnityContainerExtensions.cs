using MassTransit;
using Microsoft.Practices.Unity;
using Miles.Events;
using Miles.Persistence;
using System;

namespace Miles.MassTransit.Unity
{
    /// <exclude />
    public static class UnityContainerExtensions
    {
        /// <summary>
        /// Configures a unity container to resolve Miles MassTransit types.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="contracts">The contracts you want to handle.</param>
        /// <returns></returns>
        public static IUnityContainer LoadMilesMassTransitConfiguration(this IUnityContainer container, params Type[] contracts)
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

        /// <summary>
        /// Configures a bus into Unity once you've created it.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="busControl">The bus control.</param>
        /// <returns></returns>
        public static IUnityContainer LoadMassTransitBusConfiguration(this IUnityContainer container, IBusControl busControl)
        {
            return container
                .RegisterInstance<IBusControl>(busControl)
                .RegisterInstance<IBus>(busControl);
        }
    }
}
