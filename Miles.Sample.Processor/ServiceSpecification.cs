namespace Miles.Sample.Processor
{
    using global::MassTransit.Hosting;
    using Infrastructure.Unity;
    using MassTransit.Configuration;
    using MassTransit.Hosting;
    using MassTransit.Unity;
    using Microsoft.Practices.Unity;
    using Reflection;

    /// <summary>
    /// Configures the bus settings for the service and all endpoints in the same assembly.
    /// </summary>
    public class ServiceSpecification :
        IServiceSpecification
    {
        public void Configure(IServiceConfigurator configurator)
        {
            var container = new UnityContainer()
                .ConfigureSample(t => new HierarchicalLifetimeManager())
                .RegisterMessageProcessors(AllClasses.FromAssembliesInBasePath().GetMessageProcessors());

            global::MassTransit.ConsumerConvention.Register<MilesConvention>();
            //configurator.UseRecordMessageDispatch(c => c.UseDispatchedRepository(new DispatchedRepository()));
            configurator.UseMiles("Miles.Sample", new UnityConsumerFactoryFactory(container), AllClasses.FromAssembliesInBasePath().GetMessageProcessors());
        }
    }
}
