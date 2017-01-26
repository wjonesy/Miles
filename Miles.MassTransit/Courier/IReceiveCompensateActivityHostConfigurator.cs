using MassTransit;
using MassTransit.Courier;
using System;

namespace Miles.MassTransit.Courier
{
    public interface IReceiveCompensateActivityHostConfigurator<TActivity, TLog> : IReceiveEndpointConfigurator
        where TActivity : class, CompensateActivity<TLog>
        where TLog : class
    {
        void Activity(Action<ICompensateActivityConfigurator<TActivity, TLog>> configure = null);
    }
}
