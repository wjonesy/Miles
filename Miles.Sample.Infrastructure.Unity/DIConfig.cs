using Microsoft.Practices.Unity;
using Miles;
using Miles.MassTransit;
using Miles.MassTransit.Unity;
using Miles.Persistence;
using Miles.Sample.Persistence.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Infrastructure.Unity
{
    public static class DIConfig
    {
        public static IUnityContainer ConfigureSample(this IUnityContainer container, Func<Type, LifetimeManager> lifetimeManager)
        {
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies().Where(x => x.Namespace.StartsWith("Miles.Sample")),
                t => WithMappings.FromMatchingInterface(t),
                WithName.Default,
                lifetimeManager);

            container.RegisterType<ITime, Time>(lifetimeManager(null));
            container.RegisterMilesMassTransitCommon(() => lifetimeManager(null))
                .RegisterType<IMessageDispatcher, ConventionBasedMessageDispatcher>(lifetimeManager(null));

            container.RegisterType<ITransactionContext, SampleTransactionContext>(lifetimeManager(null));

            return container;
        }
    }
}
