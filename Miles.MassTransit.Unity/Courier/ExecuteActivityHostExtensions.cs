using MassTransit.Courier;
using Microsoft.Practices.Unity;
using Miles.MassTransit.Courier;
using Miles.MassTransit.Unity.Courier;
using System;

namespace MassTransit
{
    public static class ExecuteActivityHostExtensions
    {
        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IReceiveEndpointConfigurator configurator, Uri compensateHostAddress, IUnityContainer container, Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>, CompensateActivity<TLog>
            where TArguments : class
            where TLog : class
        {
            var compensateQueue = typeof(TLog).GenerateCompensationQueueName();
            var factory = new MilesUnityExecuteActivityFactory<TActivity, TArguments>(container);
            configurator.ExecuteActivityHost<TActivity, TArguments>(new Uri(compensateHostAddress, compensateQueue), factory, configure);
        }
    }
}
