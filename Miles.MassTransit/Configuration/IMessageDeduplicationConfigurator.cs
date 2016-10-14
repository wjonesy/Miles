namespace Miles.MassTransit.Configuration
{
    public interface IMessageDeduplicationConfigurator
    {
        IMessageDeduplicationConfigurator Enable(bool enable);
    }
}
