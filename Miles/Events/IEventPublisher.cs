namespace Miles.Events
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent evt) where TEvent : class;
    }
}
