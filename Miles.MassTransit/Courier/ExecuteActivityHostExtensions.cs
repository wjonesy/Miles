/*
 *     Copyright 2017 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
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
