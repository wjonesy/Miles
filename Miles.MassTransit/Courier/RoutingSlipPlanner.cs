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
