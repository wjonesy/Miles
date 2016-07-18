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
using MassTransit;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Miles.Messaging;
using Miles.Persistence;
using Miles.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

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
            container = container
                .AddNewExtension<Interception>();

            container = container
                .RegisterType<IEventPublisher, TransactionalMessagePublisher>(lifetimeManagerFactory(),
                    new InjectionConstructor(
                        new ResolvedParameter<ITransactionContext>(),
                        new ResolvedParameter<IOutgoingMessageRepository>(),
                        new ResolvedParameter<ITime>(),
                        new ResolvedParameter<IMessageDispatcher>(),
                        new ResolvedParameter<ConventionBasedMessageDispatcher>(),
                        new OptionalParameter<ConsumeContext>()))
                .RegisterType<ICommandPublisher, TransactionalMessagePublisher>(lifetimeManagerFactory(),
                    new InjectionConstructor(
                        new ResolvedParameter<ITransactionContext>(),
                        new ResolvedParameter<IOutgoingMessageRepository>(),
                        new ResolvedParameter<ITime>(),
                        new ResolvedParameter<IMessageDispatcher>(),
                        new ResolvedParameter<ConventionBasedMessageDispatcher>(),
                        new OptionalParameter<ConsumeContext>()))
                .RegisterType(typeof(IConsumer<>), typeof(ConsumerAdapter<>), lifetimeManagerFactory())
                .RegisterType<IConsumer<ICleanupIncomingMessages>, CleanupIncomingMessagesConsumer>(lifetimeManagerFactory())
                .RegisterType<IConsumer<ICleanupOutgoingMessages>, CleanupOutgoingMessagesConsumer>(lifetimeManagerFactory());

            container.Configure<Interception>()
                .AddPolicy("MessageProcessor.Deduplication")
                .AddMatchingRule<PreventMultipleExecutionRule>()
                .AddCallHandler<DeduplicatedMessagehandler>(lifetimeManagerFactory());

            return container;
        }

        /// <summary>
        /// Registers the message processors with unity.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="lifetimeManagerFactory">The lifetime manager factory.</param>
        /// <param name="messageProcessors">The message processors.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterMessageProcessors(this IUnityContainer container, Func<LifetimeManager> lifetimeManagerFactory, IEnumerable<Type> messageProcessors)
        {
            foreach (var messageProcessor in messageProcessors)
                container = container.RegisterMessageProcessor(lifetimeManagerFactory, messageProcessor);

            return container;
        }

        /// <summary>
        /// Registers a message processor with unity.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="lifetimeManagerFactory">The lifetime manager factory.</param>
        /// <param name="messageProcessor">The message processor.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterMessageProcessor(this IUnityContainer container, Func<LifetimeManager> lifetimeManagerFactory, Type messageProcessor)
        {
            var iMessageProcessors = messageProcessor.GetInterfaces().Where(x => x.IsMessageProcessor());
            var typeRegistration = container.RegisterType(messageProcessor);

            // Assume we want this by default
            foreach (var iMessageProcessor in iMessageProcessors)
                container = container.RegisterType(
                    iMessageProcessor,
                    messageProcessor,
                    lifetimeManagerFactory(),
                    new Interceptor<TransparentProxyInterceptor>(),
                    new InterceptionBehavior<PolicyInjectionBehavior>());

            return container;
        }
    }
}
