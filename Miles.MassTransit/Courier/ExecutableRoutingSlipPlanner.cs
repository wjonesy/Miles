using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using MassTransit.Transports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit.Courier
{
    public class ExecutableRoutingSlipPlanner<TBus> : RoutingSlipPlanner, IExecutableRoutingSlipPlanner where TBus : ISendEndpointProvider, IPublishEndpoint
    {
        private readonly TBus bus;

        public ExecutableRoutingSlipPlanner(TBus bus, IActivityTypeHostUriLookup hostLookup, IMessageNameFormatter messageNameFormatter, Guid trackingNumber) : base(hostLookup, messageNameFormatter, trackingNumber)
        {
            this.bus = bus;
        }

        public ExecutableRoutingSlipPlanner(TBus bus, IActivityTypeHostUriLookup hostLookup, IMessageNameFormatter messageNameFormatter, RoutingSlip routingSlip, Func<IEnumerable<Activity>, IEnumerable<Activity>> activitySelector)
            : base(hostLookup, messageNameFormatter, routingSlip, activitySelector)
        {
            this.bus = bus;
        }

        public ExecutableRoutingSlipPlanner(TBus bus, IActivityTypeHostUriLookup hostLookup, IMessageNameFormatter messageNameFormatter, RoutingSlip routingSlip, IEnumerable<CompensateLog> compensateLogs)
            : base(hostLookup, messageNameFormatter, routingSlip, compensateLogs)
        {
            this.bus = bus;
        }

        public ExecutableRoutingSlipPlanner(TBus bus, IActivityTypeHostUriLookup hostLookup, IMessageNameFormatter messageNameFormatter, RoutingSlip routingSlip, IEnumerable<Activity> itinerary, IEnumerable<Activity> sourceItinerary)
            : base(hostLookup, messageNameFormatter, routingSlip, itinerary, sourceItinerary)
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
