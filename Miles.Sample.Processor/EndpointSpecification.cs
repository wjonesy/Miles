using MassTransit;
using MassTransit.Hosting;
using Microsoft.Practices.Unity;
using Miles.MassTransit.Configuration;
using Miles.MassTransit.Unity;
using Miles.Reflection;
using Miles.Sample.Infrastructure.Unity;
using Miles.Sample.Persistence.EF.Access.Miles.MassTransit.RecordMessageDispatch;
using System;

namespace Miles.Sample.Processor
{
    /// <summary>
    /// Configures an endpoint for the assembly
    /// </summary>
    public class EndpointSpecification : IEndpointSpecification
    {
        /// <summary>
        /// The default queue name for the endpoint, which can be overridden in the .config 
        /// file for the assembly
        /// </summary>
        public string QueueName
        {
            get { return "Miles.Sample"; }
        }

        /// <summary>
        /// The default concurrent consumer limit for the endpoint, which can be overridden in the .config 
        /// file for the assembly
        /// </summary>
        public int ConsumerLimit
        {
            get { return Environment.ProcessorCount; }
        }

        /// <summary>
        /// Configures the endpoint, with consumers, handlers, sagas, etc.
        /// </summary>
        public void Configure(IReceiveEndpointConfigurator configurator)
        {
            var container = new UnityContainer()
                .ConfigureSample(t => new HierarchicalLifetimeManager())
                .RegisterMessageProcessors(AllClasses.FromLoadedAssemblies().GetMessageProcessors());

            configurator.UseRecordMessageDispatch(c => c.UseDispatchedRepository(new DispatchedRepository()));
            configurator.MilesConsumers(container, AllClasses.FromLoadedAssemblies().GetProcessedMessageTypes());
        }
    }
}
