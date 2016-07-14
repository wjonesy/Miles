using MassTransit;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Miles.Messaging;
using Miles.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Miles.MassTransit.Unity
{
    /// <exclude />
    public static class UnityContainerExtensions
    {
        /// <summary>
        /// Registers common components with unity.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="lifetimeManagerFactory">The lifetime manager factory.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterMilesMassTransitCommon(this IUnityContainer container, Func<LifetimeManager> lifetimeManagerFactory)
        {
            return container
                .AddNewExtension<Interception>()
                .RegisterType<IEventPublisher, TransactionalMessagePublisher>(lifetimeManagerFactory())
                .RegisterType<ICommandPublisher, TransactionalMessagePublisher>(lifetimeManagerFactory())
                .RegisterType(typeof(IConsumer<>), typeof(ConsumerAdapter<>), lifetimeManagerFactory())
                .RegisterType<IConsumer<ICleanupIncomingMessages>, CleanupIncomingMessagesConsumer>(lifetimeManagerFactory())
                .RegisterType<IConsumer<ICleanupOutgoingMessages>, CleanupOutgoingMessagesConsumer>(lifetimeManagerFactory());
        }

        public static IUnityContainer RegisterMessageProcessors(this IUnityContainer container, Func<LifetimeManager> lifetimeManagerFactory, params Type[] messageProcessors)
        {
            return container.RegisterMessageProcessors(lifetimeManagerFactory, messageProcessors);
        }

        public static IUnityContainer RegisterMessageProcessors(this IUnityContainer container, Func<LifetimeManager> lifetimeManagerFactory, IEnumerable<Type> messageProcessors)
        {
            foreach (var messageProcessor in messageProcessors)
                container = container.RegisterMessageProcessor(lifetimeManagerFactory, messageProcessor);

            return container;
        }

        public static IUnityContainer RegisterMessageProcessor(this IUnityContainer container, Func<LifetimeManager> lifetimeManagerFactory, Type messageProcessor)
        {
            var iMessageProcessors = messageProcessor.GetInterfaces().Where(x => x.IsMessageProcessor());
            var typeRegistration = container.RegisterType(messageProcessor);

            if (messageProcessor.GetCustomAttribute<PreventMultipleExecution>(true)?.Prevent ?? true)
            {
                // Assume we want this by default
                foreach (var iMessageProcessor in iMessageProcessors)
                    container = container.RegisterType(iMessageProcessor, messageProcessor, lifetimeManagerFactory(), new InterceptionBehavior<DeduplicatedMessageInterceptor>());
            }
            else
            {
                foreach (var iMessageProcessor in iMessageProcessors)
                    container = container.RegisterType(iMessageProcessor, messageProcessor, lifetimeManagerFactory());
            }

            return container;
        }
    }
}
