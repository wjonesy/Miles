// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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

using Autofac;
using MassTransit;
using MassTransit.Pipeline;
using MassTransit.Util;
using System.Threading.Tasks;

namespace Miles.MassTransit.Autofac
{
    /// <summary>
    /// Lifted from straight from MassTransit.Autofac.AutofacConsumerFactory{TConsumer} but with some additional type registration to 
    /// meet some injection requirements (specifically the consume context).
    /// </summary>
    /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
    /// <seealso cref="MassTransit.IConsumerFactory{TConsumer}" />
    class AutofacConsumerFactory<TConsumer> : IConsumerFactory<TConsumer>
        where TConsumer : class
    {
        private readonly string _name;
        private readonly ILifetimeScope _scope;

        public AutofacConsumerFactory(ILifetimeScope scope, string name)
        {
            _scope = scope;
            _name = name;
        }

        public async Task Send<TMessage>(ConsumeContext<TMessage> context, IPipe<ConsumerConsumeContext<TConsumer, TMessage>> next)
            where TMessage : class
        {
            using (var innerScope = _scope.BeginLifetimeScope(_name, c =>
                {
                    c.RegisterInstance(context)
                        .As<ConsumeContext>()
                        .As<ConsumeContext<TMessage>>()
                        .As<IPublishEndpoint>()
                        .As<ISendEndpointProvider>()
                        .ExternallyOwned();
                }))
            {
                var consumer = innerScope.Resolve<TConsumer>();
                if (consumer == null)
                {
                    throw new ConsumerException($"Unable to resolve consumer type '{TypeMetadataCache<TConsumer>.ShortName}'.");
                }

                await next.Send(context.PushConsumer(consumer)).ConfigureAwait(false);
            }
        }

        void IProbeSite.Probe(ProbeContext context)
        {
            ProbeContext scope = context.CreateConsumerFactoryScope<TConsumer>("autofac");
            scope.Add("scopeTag", _name);
        }
    }
}
