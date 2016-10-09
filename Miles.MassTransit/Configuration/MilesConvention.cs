using MassTransit;
using Miles.MassTransit.ConsumerConvention;

namespace Miles.MassTransit.Configuration
{
    public class MilesConvention : IConsumerConvention
    {
        IConsumerMessageConvention IConsumerConvention.GetConsumerMessageConvention<T>()
        {
            return new MessageProcessorMessageConvention<T>();
        }
    }
}
