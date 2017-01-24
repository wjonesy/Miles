using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit.Courier
{
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

        public async Task Execute()
        {
            var slip = this.Build();
            await bus.Execute(slip).ConfigureAwait(false);
        }
    }
}
