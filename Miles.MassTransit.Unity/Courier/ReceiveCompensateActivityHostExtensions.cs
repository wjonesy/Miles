using MassTransit.Courier;
using Microsoft.Practices.Unity;
using Miles.MassTransit.Unity.Courier;
using System;

namespace MassTransit
{
    public static class ReceiveCompensateActivityHostExtensions
    {
        public static void CompensateActivityHost<TActivity, TLog>(this IBusFactoryConfigurator configurator, IUnityContainer container, Action<IReceiveCompensateActivityHostConfigurator<TActivity, TLog>> configure = null)
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            var factory = new MilesUnityCompensateActivityFactory<TActivity, TLog>(container);
            configurator.CompensateActivityHost(factory, configure);
        }
    }
}
