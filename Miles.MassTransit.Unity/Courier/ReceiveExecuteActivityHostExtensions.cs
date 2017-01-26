using MassTransit.Courier;
using Microsoft.Practices.Unity;
using Miles.MassTransit.Unity.Courier;
using System;

namespace MassTransit
{
    public static class ReceiveExecuteActivityHostExtensions
    {
        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IBusFactoryConfigurator configurator, Uri compensateHostAddress, IUnityContainer container, Action<IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class
        {
            var factory = new MilesUnityExecuteActivityFactory<TActivity, TArguments>(container);
            configurator.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, factory, configure);
        }

        public static void ExecuteActivityHost<TActivity, TArguments>(this IBusFactoryConfigurator configurator, Uri compensateAddress, IUnityContainer container, Action<IReceiveExecuteActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class
        {
            var factory = new MilesUnityExecuteActivityFactory<TActivity, TArguments>(container);
            configurator.ExecuteActivityHost<TActivity, TArguments>(compensateAddress, factory, configure);
        }
    }
}
