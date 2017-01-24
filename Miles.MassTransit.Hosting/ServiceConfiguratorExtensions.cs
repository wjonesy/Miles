using MassTransit.Courier;
using MassTransit.Hosting;
using System;

namespace Miles.MassTransit.Hosting
{
    public static class ServiceConfiguratorExtensions
    {
        public static void ActivityHosts<TActivity, TArguments, TLog>(this IServiceConfigurator configurator, Uri compensateHostAddress, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configureExecution = null, Action<IReceiveCompensationActivityHostConfigurator<TActivity, TLog>> configureCompensation = null)
            where TActivity : class, Activity<TArguments, TLog>, new() // TODO: Overloads
            where TArguments : class
            where TLog : class
        {
            configurator.ExecutionActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, configureExecution);
            configurator.CompensateActivityHost<TActivity, TLog>(configureCompensation);
        }
    }
}
