/*
 *     Copyright 2016 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using Autofac;
using MassTransit;
using Miles.Messaging;
using Miles.Reflection;
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
        public static ContainerBuilder RegisterMilesMassTransitCommon(this ContainerBuilder builder, bool useConventionBasedCommandDispatch = true)
        {
            builder.RegisterType<TransactionalMessagePublisher>().As<IEventPublisher>().As<ICommandPublisher>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(ConsumerAdapter<>)).As(typeof(IConsumer<>)).InstancePerLifetimeScope();
            builder.RegisterType<CleanupIncomingMessagesConsumer>().As<IConsumer<ICleanupIncomingMessages>>().InstancePerLifetimeScope();
            builder.RegisterType<CleanupOutgoingMessagesConsumer>().As<IConsumer<ICleanupOutgoingMessages>>().InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(typeof(DeduplicatedMessageProcessor<>), typeof(IMessageProcessor<>), DeduplicatedMessage).InstancePerLifetimeScope();
            return builder;
        }

        public static ContainerBuilder RegisterMessageProcessors(this ContainerBuilder builder, params Type[] messageProcessors)
        {
            return builder.RegisterMessageProcessors(messageProcessors);
        }

        public static ContainerBuilder RegisterMessageProcessors(this ContainerBuilder builder, IEnumerable<Type> messageProcessors)
        {
            foreach (var messageProcessor in messageProcessors)
                builder = builder.RegisterMessageProcessor(messageProcessor);
            return builder;
        }

        public static ContainerBuilder RegisterMessageProcessor(this ContainerBuilder builder, Type messageProcessor)
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

            return builder;
        }
    }
}
