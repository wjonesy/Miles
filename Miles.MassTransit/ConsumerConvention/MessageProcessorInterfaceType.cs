using MassTransit;
using MassTransit.ConsumeConnectors;
using Miles.Messaging;
using System;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorInterfaceType<TProcessor, TMessage> : IMessageInterfaceType
        where TProcessor : class, IMessageProcessor<TMessage>
        where TMessage : class
    {
        private readonly Lazy<IMessageConnectorFactory> consumeConnectorFactory = new Lazy<IMessageConnectorFactory>(() => new MessageProcessorConnectorFactory<TProcessor, TMessage>());

        public Type MessageType => typeof(TMessage);

        public IConsumerMessageConnector GetConsumerConnector()
        {
            return consumeConnectorFactory.Value.CreateConsumerConnector();
        }

        public IInstanceMessageConnector GetInstanceConnector()
        {
            return consumeConnectorFactory.Value.CreateInstanceConnector();
        }
    }
}