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
using Microsoft.Practices.Unity;
using Miles.Messaging;
using Miles.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.Unity
{
    /// <summary>
    /// 
    /// </summary>
    public static class RegisterMilesProcessorExtensions
    {
        /// <summary>
        /// Registers the message processor with a unity container. Registers each message processor interface to the concrete implementation.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="processorType">Type of the processor.</param>
        /// <param name="injectionMembers">The injection members.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterMessageProcessor(this IUnityContainer container, Type processorType, params InjectionMember[] injectionMembers)
        {
            return container.RegisterMessageProcessor(processorType, new HierarchicalLifetimeManager(), injectionMembers);
        }

        /// <summary>
        /// Registers the message processor with a unity container. Registers each message processor interface to the concrete implementation.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="processorType">Type of the processor.</param>
        /// <param name="lifetimeManager">The lifetime manager.</param>
        /// <param name="injectionMembers">The injection members.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterMessageProcessor(this IUnityContainer container, Type processorType, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            var iProcessorTypes = processorType.GetInterfaces().Where(x => x.IsMessageProcessor());

            foreach (var iProcessorType in iProcessorTypes)
                container.RegisterType(iProcessorType, processorType);

            return container.RegisterType(processorType, lifetimeManager, injectionMembers ?? new InjectionMember[] { });
        }

        /// <summary>
        /// Registers the message processor with a unity container. Registers a specific combination of processor and message types.
        /// </summary>
        /// <typeparam name="TProcessor">The type of the processor.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="injectionMembers">The injection members.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterMessageProcessor<TProcessor, TMessage>(this IUnityContainer container, params InjectionMember[] injectionMembers)
            where TProcessor : IMessageProcessor<TMessage>
        {
            return container.RegisterType<IMessageProcessor<TMessage>, TProcessor>(new HierarchicalLifetimeManager(), injectionMembers);
        }

        /// <summary>
        /// Registers the message processor with a unity container. Registers a specific combination of processor and message types.
        /// </summary>
        /// <typeparam name="TProcessor">The type of the processor.</typeparam>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="lifetimeManager">The lifetime manager.</param>
        /// <param name="injectionMembers">The injection members.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterMessageProcessor<TProcessor, TMessage>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
            where TProcessor : IMessageProcessor<TMessage>
        {
            return container.RegisterType<IMessageProcessor<TMessage>, TProcessor>(lifetimeManager, injectionMembers);
        }

        /// <summary>
        /// Registers the message processor with a unity container. Registers each message processor interface to the concrete implementation.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="processorTypes">The processor types.</param>
        /// <param name="childConatinerLifetimeManagerFactory">The child conatiner lifetime manager factory.</param>
        /// <param name="injectionMembersFactory">The injection members factory.</param>
        /// <returns></returns>
        public static IUnityContainer RegisterMessageProcessors(this IUnityContainer container, IEnumerable<Type> processorTypes, Func<Type, LifetimeManager> childConatinerLifetimeManagerFactory = null, Func<Type, InjectionMember[]> injectionMembersFactory = null)
        {
            childConatinerLifetimeManagerFactory = childConatinerLifetimeManagerFactory ?? (t => new HierarchicalLifetimeManager());
            injectionMembersFactory = injectionMembersFactory ?? (t => null);

            foreach (var processorType in processorTypes)
                container.RegisterMessageProcessor(processorType, childConatinerLifetimeManagerFactory(processorType), injectionMembersFactory(processorType));

            return container;
        }
    }
}
