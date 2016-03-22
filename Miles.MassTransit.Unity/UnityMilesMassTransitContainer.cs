using Microsoft.Practices.Unity;

namespace Miles.MassTransit.Unity
{
    /// <summary>
    /// Adapters Unity to the requirements of the Miles.MassTransit implementation.
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
        /// Registers an instance of an object with the container.
        /// </summary>
        /// <typeparam name="TType">The type of the instance.</typeparam>
        /// <param name="instance">The instance.</param>
        public void RegisterInstance<TType>(TType instance)
        {
            container.RegisterInstance(instance);
        }

        /// <summary>
        /// Resolves a type.
        /// </summary>
        /// <typeparam name="TType">The type to resolve.</typeparam>
        /// <returns></returns>
        public TType Resolve<TType>()
        {
            return container.Resolve<TType>();
        }
    }
}
