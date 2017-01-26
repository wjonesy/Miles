using MassTransit;
using MassTransit.Courier;
using System;

namespace Miles.MassTransit.Courier
{
    public interface IReceiveExecuteActivityHostConfigurator<TActivity, TArguments> : IReceiveEndpointConfigurator
        where TActivity : class, ExecuteActivity<TArguments>
        where TArguments : class
    {
        void Activity(Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null);
    }
}
