namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageDeduplicationConfigurator
    {
        /// <summary>
        /// Enable or disable message deduplication.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> enable.</param>
        /// <returns></returns>
        IMessageDeduplicationConfigurator Enable(bool enable);
    }
}
