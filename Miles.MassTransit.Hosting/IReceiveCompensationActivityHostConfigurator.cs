using MassTransit;
using MassTransit.Courier;
using System;

namespace Miles.MassTransit.Hosting
{
    public interface IReceiveCompensationActivityHostConfigurator<TActivity, TLog> : IReceiveEndpointConfigurator
        where TActivity : class, CompensateActivity<TLog>, new()  // TODO: Overloads
        where TLog : class
    {
        void Activity(Action<ICompensateActivityConfigurator<TActivity, TLog>> configure = null);
    }
}
