using MassTransit;
using MassTransit.ConsumeConfigurators;
using Miles.MassTransit.TransactionContext;

namespace Miles.MassTransit.Configuration
{
    public static class TransactionContextExtensions
    {
        public static IConsumerConfigurator<TConsumer> UseTransactionContext<TConsumer>(this IConsumerConfigurator<TConsumer> configurator)
            where TConsumer : class, IConsumer
        {
            var spec = new TransactionContextSpecification<TConsumer>();

            configurator.AddPipeSpecification(spec);
            return configurator;
        }
    }
}
