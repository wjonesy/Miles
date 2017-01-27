using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using System;
using System.Threading.Tasks;

namespace Miles.MassTransit.Courier
{
    public interface IRoutingSlipPlanner : ItineraryBuilder
    {
        void AddActivity<TArguments>(string name, TArguments arguments) where TArguments : class;

        Task AddSubscription<TEvent>(RoutingSlipEvents events, Func<ISendEndpoint, Task> callback) where TEvent : class;

        Task AddSubscription<TEvent>(RoutingSlipEvents events, RoutingSlipEventContents contents, Func<ISendEndpoint, Task> callback) where TEvent : class;

        Task AddSubscription<TEvent>(RoutingSlipEvents events, RoutingSlipEventContents contents, string activityName, Func<ISendEndpoint, Task> callback) where TEvent : class;
    }
}