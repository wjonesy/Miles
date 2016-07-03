using Autofac;
using MassTransit;
using Miles.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Miles.MassTransit.Autofac
{
    /// <exclude />
    public static class ContainerBuilderExtensions
    {
        private const string DeduplicatedMessage = "DeduplicatedMessage";

        /// <summary>
        /// Registers common components with unity.
        /// </summary>
        /// <param name="builder">The container builder.</param>
        /// <param name="useConventionBasedCommandDispatch">if set to <c>true</c> use convention based command dispatch.</param>
        public static void RegisterMilesMassTransitCommon(this ContainerBuilder builder, bool useConventionBasedCommandDispatch = true)
        {
            builder.RegisterType<TransactionalMessagePublisher>().As<IEventPublisher>().As<ICommandPublisher>().InstancePerLifetimeScope();

            if (useConventionBasedCommandDispatch)
                builder.RegisterType<ConventionBasedMessageDispatcher>().As<ConventionBasedMessageDispatcher>().As<IMessageDispatcher>().InstancePerLifetimeScope();
            else
                builder.RegisterType<LookupBasedMessageDispatch>().As<LookupBasedMessageDispatch>().As<IMessageDispatcher>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ConsumerAdapter<>)).As(typeof(IConsumer<>)).InstancePerLifetimeScope();

            builder.RegisterType<CleanupIncomingMessagesConsumer>().As<IConsumer<ICleanupIncomingMessages>>().InstancePerLifetimeScope();
            builder.RegisterType<CleanupOutgoingMessagesConsumer>().As<IConsumer<ICleanupOutgoingMessages>>().InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(typeof(DeduplicatedMessageProcessor<>), typeof(IMessageProcessor<>), DeduplicatedMessage).InstancePerLifetimeScope();
        }

        public static void RegisterMessageProcessors(this ContainerBuilder builder, params Type[] messageProcessors)
        {
            builder.RegisterMessageProcessors(messageProcessors);
        }

        public static void RegisterMessageProcessors(this ContainerBuilder builder, IEnumerable<Type> messageProcessors)
        {
            foreach (var messageProcessor in messageProcessors)
                builder.RegisterMessageProcessor(messageProcessor);
        }

        public static void RegisterMessageProcessor(this ContainerBuilder builder, Type messageProcessor)
        {
            var iMessageProcessors = messageProcessor.GetInterfaces().Where(x => x.IsMessageProcessor());
            var typeRegistration = builder.RegisterType(messageProcessor);

            if (messageProcessor.GetCustomAttribute<PreventMultipleExecution>(true)?.Prevent ?? true)
            {
                // Assume we want this by default
                foreach (var iMessageProcessor in iMessageProcessors)
                    typeRegistration = typeRegistration.Named(DeduplicatedMessage, iMessageProcessor);
            }
            else
            {
                foreach (var iMessageProcessor in iMessageProcessors)
                    typeRegistration = typeRegistration.As(iMessageProcessor);
            }

            typeRegistration.InstancePerLifetimeScope();
        }
    }
}
