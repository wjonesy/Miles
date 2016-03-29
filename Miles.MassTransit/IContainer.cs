using MassTransit;
using Miles.Messaging;

namespace Miles.MassTransit
{
    /// <summary>
    /// Container requirements.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Registers the consume context.
        /// </summary>
        /// <remarks>
        /// It is expected this is a singleton per child container instance. It doesn't need to be cleaned up by the container.
        /// </remarks>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="instance">The instance.</param>
        void RegisterConsumeContext<TMessage>(ConsumeContext<TMessage> instance) where TMessage : class;

        /// <summary>
        /// Resolves the processor.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <returns></returns>
        IMessageProcessor<TMessage> ResolveProcessor<TMessage>();
    }
}
