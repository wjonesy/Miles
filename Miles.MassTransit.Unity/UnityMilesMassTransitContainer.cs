using MassTransit;
using Microsoft.Practices.Unity;
using Miles.Messaging;

namespace Miles.MassTransit.Unity
{
    /// <summary>
    /// Adapts Unity to the requirements of the Miles.MassTransit implementation.
    /// </summary>
    /// <seealso cref="Miles.MassTransit.IContainer" />
    class UnityMilesMassTransitContainer : IContainer
    {
        private readonly IUnityContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityMilesMassTransitContainer"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public UnityMilesMassTransitContainer(IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Registers the consume context.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <remarks>
        /// It is expected this is a singleton per child container instance. It doesn't need to be cleaned up by the container.
        /// </remarks>
        public void RegisterConsumeContext<TMessage>(ConsumeContext<TMessage> instance) where TMessage : class
        {
            this.container.RegisterInstance(instance, new ExternallyControlledLifetimeManager());
            this.container.RegisterInstance<ConsumeContext>(instance, new ExternallyControlledLifetimeManager());
        }

        /// <summary>
        /// Resolves the processor.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <returns></returns>
        public IMessageProcessor<TMessage> ResolveProcessor<TMessage>()
        {
            return container.Resolve<IMessageProcessor<TMessage>>();
        }
    }
}
