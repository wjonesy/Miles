/*
 *     Copyright 2017 Adam Burton (adz21c@gmail.com)
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
using GreenPipes;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Threading.Tasks;

namespace Miles.MassTransit.ContainerScope
{
    class ContainerScopeFilter<TContext> : IFilter<TContext> where TContext : class, PipeContext
    {
        private readonly IContainerStackFactory containerStackFactory;

        public ContainerScopeFilter(IContainerStackFactory containerStackFactory)
        {
            this.containerStackFactory = containerStackFactory;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("miles-container-scope").Add("type", containerStackFactory.ContainerType);
        }

        public async Task Send(TContext context, IPipe<TContext> next)
        {
            IContainerStack containerStack;
            if (!context.TryGetPayload(out containerStack))
            {
                if (containerStackFactory == null)
                    throw new InvalidOperationException("No container stack factory. Make sure the first ContainerScope encountered has a factory to setup the initial container.");

                containerStack = context.GetOrAddPayload(() => containerStackFactory.Create(context));
                context.GetOrAddPayload<IServiceLocator>(() => containerStack);
            }

            containerStack.PushScope(context);
            try
            {
                await next.Send(context);
            }
            finally
            {
                containerStack.PopScope();
            }
        }
    }
}
