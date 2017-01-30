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
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit.Courier
{
    /// <summary>
    /// Routing slip that takes in the bus/context to send message to.
    /// </summary>
    /// <typeparam name="TBus">The type of the bus.</typeparam>
    /// <seealso cref="Miles.MassTransit.Courier.RoutingSlipPlanner" />
    /// <seealso cref="Miles.MassTransit.Courier.IExecutableRoutingSlipPlanner" />
    public class ExecutableRoutingSlipPlanner<TBus> : RoutingSlipPlanner, IExecutableRoutingSlipPlanner where TBus : ISendEndpointProvider, IPublishEndpoint
    {
        private readonly TBus bus;

        public ExecutableRoutingSlipPlanner(TBus bus, IActivityTypeHostUriLookup hostLookup, Guid trackingNumber) : base(hostLookup, trackingNumber)
        {
            this.bus = bus;
        }

        public ExecutableRoutingSlipPlanner(TBus bus, IActivityTypeHostUriLookup hostLookup, RoutingSlip routingSlip, Func<IEnumerable<Activity>, IEnumerable<Activity>> activitySelector)
            : base(hostLookup, routingSlip, activitySelector)
        {
            this.bus = bus;
        }

        public ExecutableRoutingSlipPlanner(TBus bus, IActivityTypeHostUriLookup hostLookup, RoutingSlip routingSlip, IEnumerable<CompensateLog> compensateLogs)
            : base(hostLookup, routingSlip, compensateLogs)
        {
            this.bus = bus;
        }

        public ExecutableRoutingSlipPlanner(TBus bus, IActivityTypeHostUriLookup hostLookup, RoutingSlip routingSlip, IEnumerable<Activity> itinerary, IEnumerable<Activity> sourceItinerary)
            : base(hostLookup, routingSlip, itinerary, sourceItinerary)
        {
            this.bus = bus;
        }

        /// <summary>
        /// Executes the routing slip.
        /// </summary>
        /// <returns></returns>
        public async Task Execute()
        {
            var slip = this.Build();
            await bus.Execute(slip).ConfigureAwait(false);
        }
    }
}
