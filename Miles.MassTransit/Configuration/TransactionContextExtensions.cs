using MassTransit;
using MassTransit.ConsumeConfigurators;
using Miles.MassTransit.TransactionContext;
using Miles.Persistence;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class TransactionContextExtensions
    {
        /// <summary>
        /// Encapsulates the pipe behavior in a <see cref="ITransactionContext"/>.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
        public static IConsumerConfigurator<TConsumer> UseTransactionContext<TConsumer>(this IConsumerConfigurator<TConsumer> configurator)
            where TConsumer : class, IConsumer
        {
            var spec = new TransactionContextSpecification<TConsumer>();

            configurator.AddPipeSpecification(spec);
            return configurator;
        }
    }
}
