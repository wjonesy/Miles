using MassTransit;
using MassTransit.Courier;
using System;

namespace MassTransit
{
    public interface IReceiveCompensateActivityHostConfigurator<TActivity, TLog> : IReceiveEndpointConfigurator
        where TActivity : class, CompensateActivity<TLog>
        where TLog : class
    {
        void Activity(Action<ICompensateActivityConfigurator<TActivity, TLog>> configure = null);
    }
}
