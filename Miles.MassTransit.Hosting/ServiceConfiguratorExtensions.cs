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
    public static class ServiceConfiguratorExtensions
    {
        public static void ActivityHosts<TActivity, TArguments, TLog>(this IServiceConfigurator configurator, Uri compensateHostAddress, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments, TLog>> configureExec = null)
            where TActivity : class, Activity<TArguments, TLog>, new() // TODO: Overloads
            where TArguments : class
            where TLog : class
        {
            configurator.ExecutionActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, configureExec);
            configurator.CompensateActivityHost<TActivity, TLog>();
        }

        public static void ExecutionActivityHost<TActivity, TArguments, TLog>(this IServiceConfigurator configurator, Uri compensateHostAddress, Action<IReceiveExecutionActivityHostConfigurator<TActivity, TArguments, TLog>> configure = null)
            where TActivity : class, Activity<TArguments, TLog>, new() // TODO: Overloads
            where TArguments : class
            where TLog : class
        {
            configurator.ReceiveEndpoint(typeof(TArguments).GenerateExecutionQueueName(), r => configure?.Invoke(new ReceiveExecutionActivityHostConfigurator<TActivity, TArguments, TLog>(r, compensateHostAddress)));
        }


        public static void CompensateActivityHost<TActivity, TLog>(this IServiceConfigurator configurator)
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            configurator.ReceiveEndpoint(typeof(TLog).GenerateExecutionQueueName(), null);
        }
    }

    public interface IReceiveExecutionActivityHostConfigurator<TActivity, TArguments, TLog> : IReceiveEndpointConfigurator
        where TActivity : class, Activity<TArguments, TLog>, new()  // TODO: Overloads
        where TArguments : class
        where TLog : class
    {
        void Activity(Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null);
    }

    class ReceiveExecutionActivityHostConfigurator<TActivity, TArguments, TLog> : IReceiveExecutionActivityHostConfigurator<TActivity, TArguments, TLog>
        where TActivity : class, Activity<TArguments, TLog>, new()  // TODO: Overloads
        where TArguments : class
        where TLog : class
    {
        private readonly IReceiveEndpointConfigurator receiveEndpointConfigurator;
        private readonly Uri compensateHostAddress;

        public ReceiveExecutionActivityHostConfigurator(
            IReceiveEndpointConfigurator receiveEndpointConfigurator,
            Uri compensateHostAddress)
        {
            this.receiveEndpointConfigurator = receiveEndpointConfigurator;
            this.compensateHostAddress = compensateHostAddress;
        }

        public void Activity(Action<IExecuteActivityConfigurator<TActivity, TArguments>> configure = null)
        {
            receiveEndpointConfigurator.ExecuteActivityHost<TActivity, TArguments, TLog>(compensateHostAddress, configure);
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
