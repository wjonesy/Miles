using GreenPipes;
using MassTransit;
using MassTransit.Configuration;
using MassTransit.ConsumeConfigurators;
using MassTransit.Courier;
using MassTransit.Hosting;
using MassTransit.Saga;
using MassTransit.Saga.SubscriptionConfigurators;
using Miles.MassTransit.Courier;
using System;

namespace Miles.MassTransit.Hosting
{
    public static class ExecuteActivityHostExtensions
    {
        public static void ExecutionActivityHost<TActivity, TArguments, TLog>(this IServiceConfigurator configurator, Uri compensateHostAddress, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, Activity<TArguments, TLog>, new()
            where TArguments : class
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, ac))));
        }

        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IServiceConfigurator configurator, Uri compensateHostAddress, Func<TActivity> controllerFactory, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, controllerFactory, ac))));
        }

        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IServiceConfigurator configurator, Uri compensateHostAddress, Func<TArguments, TActivity> controllerFactory, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, controllerFactory, ac))));
        }

        public static void ExecuteActivityHost<TActivity, TArguments, TLog>(this IServiceConfigurator configurator, Uri compensateHostAddress, ExecuteActivityFactory<TActivity, TArguments> factory, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, Activity<TArguments, TLog>
            where TArguments : class
            where TLog : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, factory, ac))));
        }

        public static void ExecutionActivityHost<TActivity, TArguments>(this IServiceConfigurator configurator, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>, new() // TODO: Overloads
            where TArguments : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments>(ac))));
        }

        public static void ExecuteActivityHost<TActivity, TArguments>(this IServiceConfigurator configurator, Uri compensateHostAddress, Func<TActivity> controllerFactory, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments>(compensateHostAddress, controllerFactory, ac))));
        }

        public static void ExecuteActivityHost<TActivity, TArguments>(this IServiceConfigurator configurator, Uri compensateHostAddress, Func<TArguments, TActivity> controllerFactory, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments>(compensateHostAddress, controllerFactory, ac))));
        }

        public static void ExecuteActivityHost<TActivity, TArguments>(this IServiceConfigurator configurator, Uri compensateHostAddress, ExecuteActivityFactory<TActivity, TArguments> factory, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>> configure = null)
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class
        {
            configure = configure ?? (r => r.Activity());
            configurator.ReceiveEndpoint(
                typeof(TArguments).GenerateExecutionQueueName(),
                r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments>(r, (c, ac) => c.ExecuteActivityHost<TActivity, TArguments>(compensateHostAddress, factory, ac))));
        }

        class ReceiveExecutionActivityHostConfigurator<TActivity, TArguments> : IReceiveExecutionActivityHostConfigurator<TActivity, TArguments>
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class
        {
            private readonly IReceiveEndpointConfigurator receiveEndpointConfigurator;
            private readonly Action<IReceiveEndpointConfigurator, Action<IExecuteActivityConfigurator<TActivity, TArguments>>> activityHost;

            public ReceiveExecutionActivityHostConfigurator(
                IReceiveEndpointConfigurator receiveEndpointConfigurator,
                Action<IReceiveEndpointConfigurator, Action<IExecuteActivityConfigurator<TActivity, TArguments>>> activityHost)
            {
                this.receiveEndpointConfigurator = receiveEndpointConfigurator;
                this.activityHost = activityHost;
            }

            public void Activity(Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null)
            {
                activityHost(receiveEndpointConfigurator, configure);
            }

            #region Adapter

            public Uri InputAddress => receiveEndpointConfigurator.InputAddress;

            public void AddEndpointSpecification(IReceiveEndpointSpecification configurator)
            {
                receiveEndpointConfigurator.AddEndpointSpecification(configurator);
            }

            public void AddPipeSpecification(IPipeSpecification<ConsumeContext> specification)
            {
                receiveEndpointConfigurator.AddPipeSpecification(specification);
            }

            public void AddPipeSpecification<T>(IPipeSpecification<ConsumeContext<T>> specification) where T : class
            {
                receiveEndpointConfigurator.AddPipeSpecification(specification);
            }

            public void ConfigurePublish(Action<IPublishPipeConfigurator> callback)
            {
                receiveEndpointConfigurator.ConfigurePublish(callback);
            }

            public void ConfigureSend(Action<ISendPipeConfigurator> callback)
            {
                receiveEndpointConfigurator.ConfigureSend(callback);
            }

            public ConnectHandle ConnectConsumerConfigurationObserver(IConsumerConfigurationObserver observer)
            {
                return receiveEndpointConfigurator.ConnectConsumerConfigurationObserver(observer);
            }

            public ConnectHandle ConnectSagaConfigurationObserver(ISagaConfigurationObserver observer)
            {
                return receiveEndpointConfigurator.ConnectSagaConfigurationObserver(observer);
            }

            public void ConsumerConfigured<TConsumer>(IConsumerConfigurator<TConsumer> configurator) where TConsumer : class
            {
                receiveEndpointConfigurator.ConsumerConfigured(configurator);
            }

            public void ConsumerMessageConfigured<TConsumer, TMessage>(IConsumerMessageConfigurator<TConsumer, TMessage> configurator)
                where TConsumer : class
                where TMessage : class
            {
                receiveEndpointConfigurator.ConsumerMessageConfigured(configurator);
            }

            void ISagaConfigurationObserver.SagaConfigured<TSaga>(ISagaConfigurator<TSaga> configurator)
            {
                receiveEndpointConfigurator.SagaConfigured(configurator);
            }

            void ISagaConfigurationObserver.SagaMessageConfigured<TSaga, TMessage>(ISagaMessageConfigurator<TSaga, TMessage> configurator)
            {
                receiveEndpointConfigurator.SagaMessageConfigured(configurator);
            }

            #endregion
        }
    }
}
