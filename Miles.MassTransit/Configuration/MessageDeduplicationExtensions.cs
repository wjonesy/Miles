using MassTransit;
using MassTransit.ConsumeConfigurators;
using Microsoft.Practices.ServiceLocation;
using Miles.MassTransit.MessageDeduplication;
using Miles.Persistence;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class MessageDeduplicationExtensions
    {
        /// <summary>
        /// Wraps message in a <see cref="ITransactionContext"/> in which the message is recorded to ensure it is processed only once.
        /// On identifying a message as already processed the message is removed from the queue without doing any work.
        /// </summary>
        /// <remarks>
        /// This assumes a container will have registered itself as an <see cref="IServiceLocator"/> payload to 
        /// retrieve an <see cref="IConsumedRepository"/> instance that will work with the <see cref="ITransactionContext"/>.
        /// </remarks>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <param name="configurator">The configurator.</param>
        /// <returns></returns>
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
