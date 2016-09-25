using MassTransit;
using MassTransit.Pipeline;
using Microsoft.Practices.ServiceLocation;
using System.Threading.Tasks;

namespace Miles.MassTransit.EnsureMessageDispatch
{
    class RecordMessageDispatchFilter : IFilter<SendContext>
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("record-message-dispatch");
        }

        public async Task Send(SendContext context, IPipe<SendContext> next)
        {
            await next.Send(context).ConfigureAwait(false);

            var container = context.GetPayload<IServiceLocator>();
            var repository = container.GetInstance<IDispatchedRepository>();

            await repository.RecordAsync(context).ConfigureAwait(false);
        }
    }

    class RecordMessageDispatchFilter<TMessage> : IFilter<SendContext<TMessage>> where TMessage : class
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("record-message-dispatch");
        }

        public async Task Send(SendContext<TMessage> context, IPipe<SendContext<TMessage>> next)
        {
            await next.Send(context).ConfigureAwait(false);

            var container = context.GetPayload<IServiceLocator>();
            var repository = container.GetInstance<IDispatchedRepository>();

            await repository.RecordAsync(context).ConfigureAwait(false);
        }
    }
}
