using MassTransit;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// Creates <see cref="IConsumerFactory{TConsumer}"/>s. 
    /// </summary>
    public interface IConsumerFactoryFactory
    {
        /// <summary>
        /// Creates <see cref="IConsumerFactory{TConsumer}"/>s.
        /// </summary>
        /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
        /// <returns></returns>
        IConsumerFactory<TConsumer> CreateConsumerFactory<TConsumer>() where TConsumer : class;
    }
}