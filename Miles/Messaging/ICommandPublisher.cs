using System;
using System.Threading.Tasks;

namespace Miles.Messaging
{
    /// <summary>
    /// Represents a service that will publish commands.
    /// </summary>
    public interface ICommandPublisher
    {
        /// <summary>
        /// Registers a handler for immediate execution within a transaction.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="cmd">The command.</param>
        void Register<TCommand>(IMessageProcessor<TCommand> cmd) where TCommand : class;

        /// <summary>
        /// Publishes the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="cmd">The command.</param>
        void Publish<TCommand>(TCommand cmd) where TCommand : class;
    }

    public static class CommandPublisherExtensions
    {
        /// <summary>
        /// Registers a handler for immediate execution within a transaction.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="commandPublisher">The command publisher.</param>
        /// <param name="handler">The handler.</param>
        public static void Register<TCommand>(this ICommandPublisher commandPublisher, Func<TCommand, Task> handler) where TCommand : class
        {
            commandPublisher.Register(new CallbackMessageProcessor<TCommand>(handler));
        }
    }
}
