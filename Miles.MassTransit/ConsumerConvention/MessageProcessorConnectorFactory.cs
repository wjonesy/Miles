using MassTransit;
using MassTransit.ConsumeConnectors;
using Miles.Messaging;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorConnectorFactory<TProcessor, TMessage> : IMessageConnectorFactory
        where TProcessor : class, IMessageProcessor<TMessage>
        where TMessage : class
    {
        private readonly MessageProcessorMessageFilter<TProcessor, TMessage> filter = new MessageProcessorMessageFilter<TProcessor, TMessage>();

        public IConsumerMessageConnector CreateConsumerConnector()
        {
            return new ConsumerMessageConnector<TProcessor, TMessage>(filter);
        }

        public IInstanceMessageConnector CreateInstanceConnector()
        {
            return new InstanceMessageConnector<TProcessor, TMessage>(filter);
        }
    }
}