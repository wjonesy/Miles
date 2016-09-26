using MassTransit;
using Miles.MassTransit.EnsureMessageDispatch;
using System;

namespace Miles.MassTransit.Configuration
{
    public static class RecordMessageDispatchExtensions
    {
        public static ISendPipeConfigurator UseRecordMessageDispatch(this ISendPipeConfigurator configurator, Action<IRecordMessageDispatchConfigurator> configure)
        {
            var spec = new RecordMessageDispatchSpecification<SendContext>();
            configure.Invoke(spec);

            configurator.AddPipeSpecification(spec);
            return configurator;
        }

        public static ISendPipeConfigurator UseRecordMessageDispatch<TMessage>(this ISendPipeConfigurator configurator, Action<IRecordMessageDispatchConfigurator> configure)
            where TMessage : class
        {
            var spec = new RecordMessageDispatchSpecification<SendContext<TMessage>>();
            configure.Invoke(spec);

            configurator.AddPipeSpecification(spec);
            return configurator;
        }
    }
}
