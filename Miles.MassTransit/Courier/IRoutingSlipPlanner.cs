using MassTransit.Courier;

namespace Miles.MassTransit.Courier
{
    public interface IRoutingSlipPlanner : ItineraryBuilder
    {
        void AddActivity<TArguments>(string name, TArguments arguments) where TArguments : class;
    }
}