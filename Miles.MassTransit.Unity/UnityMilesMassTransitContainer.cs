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

namespace Miles.MassTransit.Unity
{
    /// <summary>
    /// Adapts Unity to the requirements of the Miles.MassTransit implementation.
    /// </summary>
    /// <seealso cref="Miles.MassTransit.IContainer" />
    class UnityMilesMassTransitContainer : IContainer
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityMilesMassTransitContainer"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public UnityMilesMassTransitContainer(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Registers the consume context.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <remarks>
        /// It is expected this is a singleton per child container instance. It doesn't need to be cleaned up by the container.
        /// </remarks>
        public void RegisterConsumeContext<TMessage>(ConsumeContext<TMessage> instance) where TMessage : class
        {
            this.container.RegisterInstance(instance, new ExternallyControlledLifetimeManager());
            this.container.RegisterInstance<ConsumeContext>(instance, new ExternallyControlledLifetimeManager());
        }

        /// <summary>
        /// Resolves the processor.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <returns></returns>
        public IMessageProcessor<TMessage> ResolveProcessor<TMessage>()
        {
            return container.Resolve<IMessageProcessor<TMessage>>();
        }
    }
}
