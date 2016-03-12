using Microsoft.Practices.ServiceLocation;

namespace Miles.MassTransit
{
    /// <summary>
    /// Extends <see cref="Microsoft.Practices.ServiceLocation.IServiceLocator" /> to include the ability to register instances.
    /// </summary>
    /// <seealso cref="Microsoft.Practices.ServiceLocation.IServiceLocator" />
    public interface IExtendedServiceLocator : IServiceLocator
    {
        /// <summary>
        /// Registers an instance of a type in the container.
        /// </summary>
        /// <typeparam name="TType">The type that will be resolved.</typeparam>
        /// <param name="instance">The instance.</param>
        void RegisterInstance<TType>(TType instance);
    }
}
