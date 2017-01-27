using MassTransit;
using MassTransit.Courier;
using System;

namespace MassTransit
{
    public interface IReceiveExecuteActivityHostConfigurator<TActivity, TArguments> : IReceiveEndpointConfigurator
        where TActivity : class, ExecuteActivity<TArguments>
        where TArguments : class
    {
        void Activity(Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null);
    }
}
