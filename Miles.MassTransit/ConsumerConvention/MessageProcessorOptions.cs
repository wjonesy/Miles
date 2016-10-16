using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.TransactionContext;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorOptions
    {
        public TransactionContextConfigurator TransactionContext { get; set; }
        public MessageDeduplicationConfigurator MessageDeduplication { get; set; }

        public MessageProcessorOptions Merge(MessageProcessorOptions defaults)
        {
            return new MessageProcessorOptions
            {
                TransactionContext = TransactionContext ?? defaults.TransactionContext,
                MessageDeduplication = MessageDeduplication ?? defaults.MessageDeduplication
            };
        }
    }
}
