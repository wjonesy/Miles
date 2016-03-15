namespace Miles.Events
{
    /// <summary>
    /// Represents a service that will publish commands.
    /// </summary>
    public interface ICommandPublisher
    {
        /// <summary>
        /// Publishes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="cmd">The command.</param>
        void Publish<TCommand>(TCommand cmd) where TCommand : class;
    }
}
