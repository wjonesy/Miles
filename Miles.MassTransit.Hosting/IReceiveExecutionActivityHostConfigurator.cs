using MassTransit;
using MassTransit.Courier;
using System;

namespace Miles.MassTransit.Hosting
{
    public interface IReceiveExecutionActivityHostConfigurator<TActivity, TArguments> : IReceiveEndpointConfigurator
        where TActivity : class, ExecuteActivity<TArguments>, new()  // TODO: Overloads
        where TArguments : class
    {
        void Activity(Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null);
    }
}
