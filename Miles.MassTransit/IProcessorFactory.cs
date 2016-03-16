using MassTransit;

namespace Miles.MassTransit
{
    public interface IProcessorFactory
    {
        IEventProcessor<TEvent> CreateEventProcessor<TEvent>(ConsumeContext consumerContext);
    }
}
