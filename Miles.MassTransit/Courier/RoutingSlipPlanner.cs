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
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Courier
{
    public class RoutingSlipPlanner : RoutingSlipBuilder, IRoutingSlipPlanner
    {
        private readonly IActivityTypeHostUriLookup hostLookup;

        public RoutingSlipPlanner(IActivityTypeHostUriLookup hostLookup, Guid trackingNumber) : base(trackingNumber)
        {
            this.hostLookup = hostLookup;
        }

        public RoutingSlipPlanner(IActivityTypeHostUriLookup hostLookup, RoutingSlip routingSlip, Func<IEnumerable<Activity>, IEnumerable<Activity>> activitySelector)
            : base(routingSlip, activitySelector)
        {
            this.hostLookup = hostLookup;
        }

        public RoutingSlipPlanner(IActivityTypeHostUriLookup hostLookup, RoutingSlip routingSlip, IEnumerable<CompensateLog> compensateLogs)
            : base(routingSlip, compensateLogs)
        {
            this.hostLookup = hostLookup;
        }

        public RoutingSlipPlanner(IActivityTypeHostUriLookup hostLookup, RoutingSlip routingSlip, IEnumerable<Activity> itinerary, IEnumerable<Activity> sourceItinerary)
            : base(routingSlip, itinerary, sourceItinerary)
        {
            this.hostLookup = hostLookup;
        }

        public void AddActivity<TArguments>(string name, TArguments arguments) where TArguments : class
        {
            var hostAddress = hostLookup.Lookup<TArguments>();
            this.AddActivity<TArguments>(name, hostAddress, arguments);
        }
    }
}
