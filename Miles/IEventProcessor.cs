namespace Miles
{
    public interface IEventProcessor<in TEvent>
    {
        void Process(TEvent evt);
    }
}
