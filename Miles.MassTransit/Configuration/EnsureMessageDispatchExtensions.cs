using MassTransit;
using Miles.MassTransit.EnsureMessageDispatch;
using System;

namespace Miles.MassTransit.Configuration
{
    public static class EnsureMessageDispatchExtensions
    {
        public static TConfigurator UseEnsureMessageDispatch<TConfigurator>(this TConfigurator configurator, Action<IRecordMessageDispatchConfigurator> configure)
            where TConfigurator : IPublishPipelineConfigurator, ISendPipelineConfigurator
        {
            var spec = new RecordMessageDispatchSpecification<SendContext>();
            configure.Invoke(spec);

            configurator.ConfigureSend(s => s.AddPipeSpecification(spec));
            configurator.ConfigurePublish(p => p.AddPipeSpecification(spec));
            return configurator;
        }
    }
}
