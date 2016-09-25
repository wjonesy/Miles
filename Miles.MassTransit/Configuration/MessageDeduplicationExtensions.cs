using MassTransit;
using MassTransit.ConsumeConfigurators;
using Miles.MassTransit.MessageDeduplication;

namespace Miles.MassTransit.Configuration
{
    public static class MessageDeduplicationExtensions
    {
        public static IConsumerConfigurator<TConsumer> UseMessageDeduplication<TConsumer>(this IConsumerConfigurator<TConsumer> configurator)
            where TConsumer : class, IConsumer
        {
            // Transaction is required to ensure recording and
            // processing of message are a single unit of work
            configurator.UseTransactionContext();

            var spec = new MessageDeduplicationSpecification<TConsumer>();
            configurator.AddPipeSpecification(spec);
            return configurator;
        }
    }
}
