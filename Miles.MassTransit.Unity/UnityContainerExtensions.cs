using MassTransit;
using Microsoft.Practices.Unity;
using Miles.Events;
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
        public static IUnityContainer LoadMilesMassTransitConfiguration(this IUnityContainer container, Func<LifetimeManager> lifetimeManagerFactory, bool useConventionBasedCommandDispatch = true, params Type[] contracts)
        {
            var genericIConsumerType = typeof(IConsumer<>);
            var genericConsumerType = typeof(MassTransitConsumer<>);

            foreach (var contract in contracts)
            {
                var iconsumerContract = genericIConsumerType.MakeGenericType(contract);
                var consumerContract = genericConsumerType.MakeGenericType(contract);
                container.RegisterType(iconsumerContract, consumerContract, lifetimeManagerFactory());
            }

            if (useConventionBasedCommandDispatch)
                container.RegisterType<IMessageDispatcher, ConventionBasedMessageDispatcher>(
                    lifetimeManagerFactory(),
                    new InjectionConstructor(
                        new ResolvedParameter<IBus>(),
                        new OptionalParameter<ConsumeContext>()));
            else
                container.RegisterType<IMessageDispatcher, LookupBasedMessageDispatch>(
                    lifetimeManagerFactory(),
                    new InjectionConstructor(
                        new ResolvedParameter<ILookupEndpointUri>(),
                        new ResolvedParameter<IBus>(),
                        new OptionalParameter<ConsumeContext>()));

            return container
                .RegisterType<IProcessorFactory, ProcessorFactory>(lifetimeManagerFactory())
                .RegisterType<IEventPublisher, TransactionalMessagePublisher>(lifetimeManagerFactory())
                .RegisterType<ICommandPublisher, TransactionalMessagePublisher>(lifetimeManagerFactory());
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
