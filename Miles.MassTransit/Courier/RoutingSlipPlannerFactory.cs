﻿/*
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
using MassTransit.Courier.Contracts;
using MassTransit.Transports;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Courier
{
    public class RoutingSlipPlannerFactory<TBus> : IRoutingSlipPlannerFactory where TBus : class, ISendEndpointProvider, IPublishEndpoint
    {
        private readonly TBus bus;
        private readonly IActivityTypeHostUriLookup hostLookup;
        private readonly IMessageNameFormatter messageNameFormatter;

        public RoutingSlipPlannerFactory(TBus bus, IActivityTypeHostUriLookup hostLookup, IMessageNameFormatter messageNameFormatter)
        {
            this.bus = bus;
            this.hostLookup = hostLookup;
            this.messageNameFormatter = messageNameFormatter;
        }

        public IExecutableRoutingSlipPlanner Create(Guid trackingNumber)
        {
            return new ExecutableRoutingSlipPlanner<TBus>(bus, hostLookup, messageNameFormatter, trackingNumber);
        }

        public IExecutableRoutingSlipPlanner Create(RoutingSlip routingSlip, IEnumerable<CompensateLog> compensateLogs)
        {
            return new ExecutableRoutingSlipPlanner<TBus>(bus, hostLookup, messageNameFormatter, routingSlip, compensateLogs);
        }

        public IExecutableRoutingSlipPlanner Create(RoutingSlip routingSlip, Func<IEnumerable<Activity>, IEnumerable<Activity>> activitySelector)
        {
            return new ExecutableRoutingSlipPlanner<TBus>(bus, hostLookup, messageNameFormatter, routingSlip, activitySelector);
        }

        public IExecutableRoutingSlipPlanner Create(RoutingSlip routingSlip, IEnumerable<Activity> itinerary, IEnumerable<Activity> sourceItinerary)
        {
            return new ExecutableRoutingSlipPlanner<TBus>(bus, hostLookup, messageNameFormatter, routingSlip, itinerary, sourceItinerary);
        }
    }
}
