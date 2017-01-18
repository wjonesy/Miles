using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.ConsumeConnectors;
using MassTransit.Logging;
using MassTransit.Pipeline;
using MassTransit.Pipeline.ConsumerFactories;
using MassTransit.Util;
using Miles.MassTransit.ConsumerConvention;
using Miles.Messaging;
using System;

namespace Miles.MassTransit.Configuration
{
    /// <exclude />
    public static class MessageProcessorExtensions
    {
        static readonly ILog _log = Logger.Get(typeof(MessageProcessorExtensions));

        /// <summary>
        /// Connect a consumer to the receiving endpoint
        /// </summary>
        /// <typeparam name="TProcessor"></typeparam>
        /// <param name="configurator"></param>
        /// <param name="consumerFactory"></param>
        /// <param name="configure">Optional, configure the consumer</param>
        /// <returns></returns>
        public static void MessageProcessor<TProcessor>(this IReceiveEndpointConfigurator configurator, IConsumerFactory<TProcessor> consumerFactory, Action<IConsumerConfigurator<TProcessor>> configure = null)
            where TProcessor : class, IMessageProcessor
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Subscribing Consumer: {0} (using supplied consumer factory)", TypeMetadataCache<TProcessor>.ShortName);

            var consumerConfigurator = new MessageProcessorConfigurator<TProcessor>(consumerFactory, configurator);

            configure?.Invoke(consumerConfigurator);

            configurator.AddEndpointSpecification(consumerConfigurator);
        }

        /// <summary>
        /// Connect a consumer to the bus instance's default endpoint
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connector"></param>
        /// <param name="consumerFactory"></param>
        /// <param name="pipeSpecifications"></param>
        /// <returns></returns>
        public static ConnectHandle ConnectMessageProcessor<T>(this IConsumePipeConnector connector, IConsumerFactory<T> consumerFactory, params IPipeSpecification<ConsumerConsumeContext<T>>[] pipeSpecifications)
            where T : class, IMessageProcessor
        {
            if (connector == null)
                throw new ArgumentNullException(nameof(connector));
            if (consumerFactory == null)
                throw new ArgumentNullException(nameof(consumerFactory));

            IConsumerSpecification<T> specification = ConsumerConnectorCache<T>.Connector.CreateConsumerSpecification<T>();
            foreach (IPipeSpecification<ConsumerConsumeContext<T>> pipeSpecification in pipeSpecifications)
            {
                specification.AddPipeSpecification(pipeSpecification);
            }
            return ConsumerConnectorCache<T>.Connector.ConnectConsumer(connector, consumerFactory, specification);
        }

        /// <summary>
        /// Subscribes a consumer with a default constructor to the endpoint
        /// </summary>
        /// <typeparam name="TProcessor">The consumer type</typeparam>
        /// <param name="configurator"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static void MessageProcessor<TProcessor>(this IReceiveEndpointConfigurator configurator, Action<IConsumerConfigurator<TProcessor>> configure = null)
            where TProcessor : class, IMessageProcessor, new()
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Subscribing Consumer: {0} (using default constructor)", TypeMetadataCache<TProcessor>.ShortName);

            var consumerFactory = new DefaultConstructorConsumerFactory<TProcessor>();

            configurator.MessageProcessor(consumerFactory, configure);
        }

        /// <summary>
        /// Subscribe a consumer with a default constructor to the bus's default endpoint
        /// </summary>
        /// <typeparam name="TProcessor"></typeparam>
        /// <param name="connector"></param>
        /// <param name="pipeSpecifications"></param>
        /// <returns></returns>
        public static ConnectHandle ConnectMessageProcessor<TProcessor>(this IConsumePipeConnector connector, params IPipeSpecification<ConsumerConsumeContext<TProcessor>>[] pipeSpecifications)
            where TProcessor : class, IMessageProcessor, new()
        {
            if (connector == null)
                throw new ArgumentNullException(nameof(connector));

            return ConnectMessageProcessor(connector, new DefaultConstructorConsumerFactory<TProcessor>(), pipeSpecifications);
        }

        /// <summary>
        /// Connect a consumer with a consumer factory method
        /// </summary>
        /// <typeparam name="TProcessor"></typeparam>
        /// <param name="configurator"></param>
        /// <param name="consumerFactoryMethod"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static void MessageProcessor<TProcessor>(this IReceiveEndpointConfigurator configurator, Func<TProcessor> consumerFactoryMethod, Action<IConsumerConfigurator<TProcessor>> configure = null)
            where TProcessor : class, IMessageProcessor
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Subscribing Consumer: {0} (using delegate consumer factory)", TypeMetadataCache<TProcessor>.ShortName);

            var delegateConsumerFactory = new DelegateConsumerFactory<TProcessor>(consumerFactoryMethod);

            configurator.MessageProcessor(delegateConsumerFactory, configure);
        }

        /// <summary>
        /// Subscribe a consumer with a consumer factor method to the bus's default endpoint
        /// </summary>
        /// <typeparam name="TProcessor"></typeparam>
        /// <param name="connector"></param>
        /// <param name="consumerFactoryMethod"></param>
        /// <param name="pipeSpecifications"></param>
        /// <returns></returns>
        public static ConnectHandle ConnectMessageProcessor<TProcessor>(this IConsumePipeConnector connector, Func<TProcessor> consumerFactoryMethod, params IPipeSpecification<ConsumerConsumeContext<TProcessor>>[] pipeSpecifications)
            where TProcessor : class, IMessageProcessor
        {
            if (_log.IsDebugEnabled)
                _log.DebugFormat("Subscribing Consumer: {0} (using delegate consumer factory)", typeof(TProcessor));

            var consumerFactory = new DelegateConsumerFactory<TProcessor>(consumerFactoryMethod);

            return ConnectMessageProcessor(connector, consumerFactory, pipeSpecifications);
        }
    }
}
