using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using MassTransit.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task AddSubscription<TEvent>(RoutingSlipEvents events, Func<ISendEndpoint, Task> callback) where TEvent : class
        {
            var hostAddress = hostLookup.Lookup<TEvent>();
            var typeName = typeof(TEvent).GetTypeName();
            var address = new Uri(hostAddress, typeName);
            await AddSubscription(address, events, callback);
        }

        public async Task AddSubscription<TEvent>(RoutingSlipEvents events, RoutingSlipEventContents contents, Func<ISendEndpoint, Task> callback) where TEvent : class
        {
            var hostAddress = hostLookup.Lookup<TEvent>();
            var typeName = typeof(TEvent).GetTypeName();
            var address = new Uri(hostAddress, typeName);
            await AddSubscription(address, events, contents, callback);
        }

        public async Task AddSubscription<TEvent>(RoutingSlipEvents events, RoutingSlipEventContents contents, string activityName, Func<ISendEndpoint, Task> callback) where TEvent : class
        {
            var hostAddress = hostLookup.Lookup<TEvent>();
            var typeName = typeof(TEvent).GetTypeName();
            var address = new Uri(hostAddress, typeName);
            await AddSubscription(address, events, contents, activityName, callback);
        }
    }
}
