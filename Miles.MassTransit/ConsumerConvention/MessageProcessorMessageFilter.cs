using MassTransit;
using MassTransit.Pipeline;
using MassTransit.Util;
using Miles.Messaging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorMessageFilter<TProcessor, TMessage> : IConsumerMessageFilter<TProcessor, TMessage>
        where TProcessor : class, IMessageProcessor<TMessage>
        where TMessage : class
    {
        void IProbeSite.Probe(ProbeContext context)
        {
            var scope = context.CreateScope("miles-message-processor");
            scope.Add("method", $"ProcessAsync({TypeMetadataCache<TMessage>.ShortName} message)");
        }

        [DebuggerNonUserCode]
        Task IFilter<ConsumerConsumeContext<TProcessor, TMessage>>.Send(
            ConsumerConsumeContext<TProcessor, TMessage> context,
            IPipe<ConsumerConsumeContext<TProcessor, TMessage>> next)
        {
            var messageProcessor = context.Consumer as IMessageProcessor<TMessage>;
            if (messageProcessor == null)
            {
                string message = $"Message Processor type {TypeMetadataCache<TProcessor>.ShortName} is not a consumer of message type {TypeMetadataCache<TMessage>.ShortName}";

                throw new ConsumerMessageException(message);
            }

            return messageProcessor.ProcessAsync(context.Message);
        }
    }
}