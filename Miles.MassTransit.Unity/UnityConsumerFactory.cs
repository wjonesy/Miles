// Copyright 2007-2016 Adam Burton, Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
using GreenPipes;
using MassTransit;
using MassTransit.Pipeline;
using MassTransit.Util;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;

namespace Miles.MassTransit.Unity
{
    /// <summary>
    /// Lifted straight from MassTransit but with some additional type registration to 
    /// meet some injection requirements (specifically the ConsumeContext).
    /// </summary>
    /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
    /// <seealso cref="global::MassTransit.IConsumerFactory{TConsumer}" />
    class UnityConsumerFactory<TConsumer> : IConsumerFactory<TConsumer> where TConsumer : class
    {
        readonly IUnityContainer container;

        public UnityConsumerFactory(IUnityContainer container)
        {
            this.container = container;
        }

        public async Task Send<T>(ConsumeContext<T> context, IPipe<ConsumerConsumeContext<TConsumer, T>> next) where T : class
        {
            using (IUnityContainer childContainer = container.CreateChildContainer())
            {
                context.GetOrAddPayload<IServiceLocator>(() => new UnityServiceLocator(childContainer));

                // register with the container for injection
                childContainer.RegisterInstance<ConsumeContext>(context)
                    .RegisterInstance<ConsumeContext<T>>(context)
                    .RegisterInstance<IPublishEndpoint>(context)
                    .RegisterInstance<ISendEndpointProvider>(context);

                var consumer = childContainer.Resolve<TConsumer>();
                if (consumer == null)
                {
                    throw new ConsumerException($"Unable to resolve consumer type '{TypeMetadataCache<TConsumer>.ShortName}'.");
                }

                await next.Send(context.PushConsumer(consumer)).ConfigureAwait(false);
            }
        }

        void IProbeSite.Probe(ProbeContext context)
        {
            context.CreateConsumerFactoryScope<TConsumer>("miles-unity");
        }
    }
}