using MassTransit;
using Miles.MassTransit.ConsumerConvention;

namespace Miles.MassTransit.Configuration
{
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="MassTransit.IConsumerConvention" />
    public class MilesConvention : IConsumerConvention
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
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
