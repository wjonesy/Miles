using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using MassTransit.Transports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit.Courier
{
    public class RoutingSlipPlanner : RoutingSlipBuilder, IRoutingSlipPlanner
    {
        private readonly IActivityTypeHostUriLookup hostLookup;
        private readonly IMessageNameFormatter messageNameFormatter;

        public RoutingSlipPlanner(IActivityTypeHostUriLookup hostLookup, IMessageNameFormatter messageNameFormatter, Guid trackingNumber) : base(trackingNumber)
        {
            this.hostLookup = hostLookup;
            this.messageNameFormatter = messageNameFormatter;
        }

        public RoutingSlipPlanner(IActivityTypeHostUriLookup hostLookup, IMessageNameFormatter messageNameFormatter, RoutingSlip routingSlip, Func<IEnumerable<Activity>, IEnumerable<Activity>> activitySelector)
            : base(routingSlip, activitySelector)
        {
            this.hostLookup = hostLookup;
            this.messageNameFormatter = messageNameFormatter;
        }

        public RoutingSlipPlanner(IActivityTypeHostUriLookup hostLookup, IMessageNameFormatter messageNameFormatter, RoutingSlip routingSlip, IEnumerable<CompensateLog> compensateLogs)
            : base(routingSlip, compensateLogs)
        {
            this.hostLookup = hostLookup;
            this.messageNameFormatter = messageNameFormatter;
        }

        public RoutingSlipPlanner(IActivityTypeHostUriLookup hostLookup, IMessageNameFormatter messageNameFormatter, RoutingSlip routingSlip, IEnumerable<Activity> itinerary, IEnumerable<Activity> sourceItinerary)
            : base(routingSlip, itinerary, sourceItinerary)
        {
            this.hostLookup = hostLookup;
            this.messageNameFormatter = messageNameFormatter;
        }

        public void AddActivity<TArguments>(string name, TArguments arguments) where TArguments : class
        {
            var hostAddress = hostLookup.Lookup<TArguments>();
            this.AddActivity<TArguments>(name, hostAddress, arguments);
        }

        public async Task AddSubscription<TEvent>(RoutingSlipEvents events, Func<ISendEndpoint, Task> callback) where TEvent : class
        {
            var hostAddress = hostLookup.Lookup<TEvent>();
            var typeName = messageNameFormatter.GetMessageName(typeof(TEvent)).Name;
            var address = new Uri(hostAddress, typeName);
            await AddSubscription(address, events, callback);
        }

        public async Task AddSubscription<TEvent>(RoutingSlipEvents events, RoutingSlipEventContents contents, Func<ISendEndpoint, Task> callback) where TEvent : class
        {
            var hostAddress = hostLookup.Lookup<TEvent>();
            var typeName = messageNameFormatter.GetMessageName(typeof(TEvent)).Name;
            var address = new Uri(hostAddress, typeName);
            await AddSubscription(address, events, contents, callback);
        }

        public async Task AddSubscription<TEvent>(RoutingSlipEvents events, RoutingSlipEventContents contents, string activityName, Func<ISendEndpoint, Task> callback) where TEvent : class
        {
            var hostAddress = hostLookup.Lookup<TEvent>();
            var typeName = messageNameFormatter.GetMessageName(typeof(TEvent)).Name;
            var address = new Uri(hostAddress, typeName);
            await AddSubscription(address, events, contents, activityName, callback);
        }
    }
}
