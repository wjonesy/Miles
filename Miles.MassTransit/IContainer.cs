namespace Miles.MassTransit
{
    /// <summary>
    /// Container requirements.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Registers an instance of an object with the container.
        /// </summary>
        /// <typeparam name="TType">The type of the instance.</typeparam>
        /// <param name="instance">The instance.</param>
        void RegisterInstance<TType>(TType instance);

        /// <summary>
        /// Resolves a type.
        /// </summary>
        /// <typeparam name="TType">The type to resolve.</typeparam>
        /// <returns></returns>
        TType Resolve<TType>();
    }
}
