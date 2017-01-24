using MassTransit;
using MassTransit.Courier.Contracts;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Courier
{
    public class RoutingSlipPlannerFactory<TBus> : IRoutingSlipPlannerFactory where TBus : class, ISendEndpointProvider, IPublishEndpoint
    {
        private readonly TBus bus;
        private readonly IActivityTypeHostUriLookup hostLookup;

        public RoutingSlipPlannerFactory(TBus bus, IActivityTypeHostUriLookup hostLookup)
        {
            this.bus = bus;
            this.hostLookup = hostLookup;
        }

        public IExecutableRoutingSlipPlanner Create(Guid trackingNumber)
        {
            return new ExecutableRoutingSlipPlanner<TBus>(bus, hostLookup, trackingNumber);
        }

        public IExecutableRoutingSlipPlanner Create(RoutingSlip routingSlip, IEnumerable<CompensateLog> compensateLogs)
        {
            return new ExecutableRoutingSlipPlanner<TBus>(bus, hostLookup, routingSlip, compensateLogs);
        }

        public IExecutableRoutingSlipPlanner Create(RoutingSlip routingSlip, Func<IEnumerable<Activity>, IEnumerable<Activity>> activitySelector)
        {
            return new ExecutableRoutingSlipPlanner<TBus>(bus, hostLookup, routingSlip, activitySelector);
        }

        public IExecutableRoutingSlipPlanner Create(RoutingSlip routingSlip, IEnumerable<Activity> itinerary, IEnumerable<Activity> sourceItinerary)
        {
            return new ExecutableRoutingSlipPlanner<TBus>(bus, hostLookup, routingSlip, itinerary, sourceItinerary);
        }
    }
}
