using MassTransit.Courier.Contracts;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Courier
{
    public interface IRoutingSlipPlannerFactory
    {
        IExecutableRoutingSlipPlanner Create(Guid trackingNumber);

        IExecutableRoutingSlipPlanner Create(RoutingSlip routingSlip, Func<IEnumerable<Activity>, IEnumerable<Activity>> activitySelector);

        IExecutableRoutingSlipPlanner Create(RoutingSlip routingSlip, IEnumerable<CompensateLog> compensateLogs);

        IExecutableRoutingSlipPlanner Create(RoutingSlip routingSlip, IEnumerable<Activity> itinerary, IEnumerable<Activity> sourceItinerary);
    }
}
