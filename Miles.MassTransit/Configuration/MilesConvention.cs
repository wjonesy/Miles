using MassTransit;
using Miles.MassTransit.ConsumerConvention;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="global::MassTransit.IConsumerConvention" />
    public class MilesConvention : IConsumerConvention
    {
        /// <summary>
        /// Returns the message convention for the type of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IConsumerMessageConvention IConsumerConvention.GetConsumerMessageConvention<T>()
        {
            return new MessageProcessorMessageConvention<T>();
        }
    }
}
