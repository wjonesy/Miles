using Microsoft.Practices.ServiceLocation;

namespace Miles.MassTransit
{
    public interface IExtendedServiceLocator : IServiceLocator
    {
        void RegisterInstance<TType>(TType instance);
    }
}
