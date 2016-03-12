using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Miles.MassTransit.Unity
{
    public class UnityExtendedServiceLocator : IExtendedServiceLocator
    {
        private readonly UnityServiceLocator serviceLocator;
        private readonly IUnityContainer container;

        public UnityExtendedServiceLocator(UnityServiceLocator serviceLocator, IUnityContainer container)
        {
            this.serviceLocator = serviceLocator;
            this.container = container;
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return serviceLocator.GetAllInstances(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return serviceLocator.GetAllInstances<TService>();
        }

        public object GetInstance(Type serviceType)
        {
            return serviceLocator.GetInstance(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return serviceLocator.GetInstance(serviceType, key);
        }

        public TService GetInstance<TService>()
        {
            return serviceLocator.GetInstance<TService>();
        }

        public TService GetInstance<TService>(string key)
        {
            return serviceLocator.GetInstance<TService>(key);
        }

        public object GetService(Type serviceType)
        {
            return serviceLocator.GetService(serviceType);
        }

        public void RegisterInstance<TType>(TType instance)
        {
            container.RegisterInstance<TType>(instance);
        }
    }
}
