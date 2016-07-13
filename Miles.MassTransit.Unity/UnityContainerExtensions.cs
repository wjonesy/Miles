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
using Miles.Messaging;
using System;

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
                .RegisterType<IContainer, UnityMilesMassTransitContainer>(lifetimeManagerFactory())
                .RegisterType<IEventPublisher, TransactionalMessagePublisher>(lifetimeManagerFactory())
                .RegisterType<ICommandPublisher, TransactionalMessagePublisher>(lifetimeManagerFactory())
                .RegisterType<IConsumer<ICleanupIncomingMessages>, CleanupIncomingMessagesConsumer>(lifetimeManagerFactory())
                .RegisterType<IConsumer<ICleanupOutgoingMessages>, CleanupOutgoingMessagesConsumer>(lifetimeManagerFactory());
        }

        /// <summary>
        /// Registers the message contracts you intend for an endpoint to resolve with message deduplication.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="lifetimeManagerFactory">The lifetime manager factory.</param>
        /// <param name="contracts">The contracts.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterDeduplicatedContracts(this IUnityContainer container, Func<LifetimeManager> lifetimeManagerFactory, params Type[] contracts)
        {
            var genericIConsumerType = typeof(IConsumer<>);
            var genericConsumerType = typeof(DeduplicatedConsumer<>);

            foreach (var contract in contracts)
            {
                var iconsumerContract = genericIConsumerType.MakeGenericType(contract);
                var consumerContract = genericConsumerType.MakeGenericType(contract);
                container.RegisterType(iconsumerContract, consumerContract, lifetimeManagerFactory());
            }

            return container;
        }

        /// <summary>
        /// Registers the message contracts you intend for an endpoint to resolve.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="lifetimeManagerFactory">The lifetime manager factory.</param>
        /// <param name="contracts">The contracts.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterContracts(this IUnityContainer container, Func<LifetimeManager> lifetimeManagerFactory, params Type[] contracts)
        {
            var genericIConsumerType = typeof(IConsumer<>);
            var genericConsumerType = typeof(Consumer<>);

            foreach (var contract in contracts)
            {
                var iconsumerContract = genericIConsumerType.MakeGenericType(contract);
                var consumerContract = genericConsumerType.MakeGenericType(contract);
                container.RegisterType(iconsumerContract, consumerContract, lifetimeManagerFactory());
            }

            return container;
        }

        /// <summary>
        /// Registers the command publisher.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="lifetimeManagerFactory">The lifetime manager factory.</param>
        /// <param name="useConventionBasedCommandDispatch">if set to <c>true</c> use the convention based command dispatchr.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterCommandDispatcher(this IUnityContainer container, Func<LifetimeManager> lifetimeManagerFactory, bool useConventionBasedCommandDispatch = true)
        {
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

            return container;
        }

        /// <summary>
        /// Registers an instance of the bus with Unity.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="busControl">The bus control.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterMassTransitBus(this IUnityContainer container, IBusControl busControl)
        {
            return container
                .RegisterInstance<IBusControl>(busControl)
                .RegisterInstance<IBus>(busControl);
        }
    }
}
