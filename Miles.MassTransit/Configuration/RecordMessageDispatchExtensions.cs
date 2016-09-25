using MassTransit;
using Miles.MassTransit.EnsureMessageDispatch;

namespace Miles.MassTransit.Configuration
{
    public static class RecordMessageDispatchExtensions
    {
        public static ISendPipeConfigurator UseEnsureMessageDispatch(this ISendPipeConfigurator configurator)
        {
            var spec = new RecordMessageDispatchSpecification();
            configurator.AddPipeSpecification(spec);
            return configurator;
        }

        public static ISendPipeConfigurator UseEnsureMessageDispatch<TMessage>(this ISendPipeConfigurator configurator)
            where TMessage : class
        {
            var spec = new RecordMessageDispatchSpecification<TMessage>();
            configurator.AddPipeSpecification<TMessage>(spec);
            return configurator;
        }
    }
}
