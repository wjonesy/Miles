using MassTransit.Courier;
using Miles.MassTransit.Courier;
using System;

namespace MassTransit
{
    public static class ExecuteActivityHostExtensions
    {
        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IReceiveEndpointConfigurator configurator, Uri compensateHostAddress, Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>, CompensateActivity<TLog>, new()
            where TArguments : class
            where TLog : class
        {
            var compensateQueue = typeof(TLog).GenerateCompensationQueueName();
            configurator.ExecuteActivityHost<TActivity, TArguments>(new Uri(compensateHostAddress, compensateQueue), configure);
        }

        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IReceiveEndpointConfigurator configurator, Uri compensateHostAddress, Func<TActivity> controllerFactory, Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>, CompensateActivity<TLog>
            where TArguments : class
            where TLog : class
        {
            var compensateQueue = typeof(TLog).GenerateCompensationQueueName();
            configurator.ExecuteActivityHost<TActivity, TArguments>(new Uri(compensateHostAddress, compensateQueue), controllerFactory, configure);
        }

        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IReceiveEndpointConfigurator configurator, Uri compensateHostAddress, Func<TArguments, TActivity> controllerFactory, Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>, CompensateActivity<TLog>
            where TArguments : class
            where TLog : class
        {
            var compensateQueue = typeof(TLog).GenerateCompensationQueueName();
            configurator.ExecuteActivityHost<TActivity, TArguments>(new Uri(compensateHostAddress, compensateQueue), controllerFactory, configure);
        }

        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IReceiveEndpointConfigurator configurator, Uri compensateHostAddress, ExecuteActivityFactory<TActivity, TArguments> factory, Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>, CompensateActivity<TLog>
            where TArguments : class
            where TLog : class
        {
            var compensateQueue = typeof(TLog).GenerateCompensationQueueName();
            configurator.ExecuteActivityHost<TActivity, TArguments>(new Uri(compensateHostAddress, compensateQueue), factory, configure);
        }
    }
}
