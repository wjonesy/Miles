using Microsoft.Practices.Unity;
using Miles.MassTransit.EntityFramework;
using Miles.MassTransit.Unity;
using Miles.Persistence;
using Miles.Sample.Persistence.EF;
using System;
using System.Data.Entity;
using System.Linq;

namespace Miles.Sample.Infrastructure.Unity
{
    public static class DIConfig
    {
        public static IUnityContainer ConfigureSample(this IUnityContainer container, Func<Type, LifetimeManager> lifetimeManager)
        {
            container.RegisterTypes(
                AllClasses.FromAssembliesInBasePath().Where(x => x.Namespace.StartsWith("Miles.Sample")),
                t => WithMappings.FromMatchingInterface(t),
                WithName.Default,
                lifetimeManager);

            // Miles.MassTransit EF repositories
            container.RegisterTypes(
                AllClasses.FromAssembliesInBasePath().Where(x => x.Namespace.StartsWith("Miles.MassTransit.EntityFramework.Repositories")),
                t => WithMappings.FromMatchingInterface(t),
                WithName.Default,
                lifetimeManager);

            container.RegisterType<DbContext, SampleDbContext>(lifetimeManager(null));
            container.RegisterType<ITime, Time>(lifetimeManager(null));
            container.RegisterMilesMassTransit(new UnityRegistrationConfiguration { ChildContainerLifetimeManagerFactory = lifetimeManager });

            container.RegisterType<ITransactionContext, EFTransactionContext>(lifetimeManager(null));

            return container;
        }
    }
}
