using MassTransit;
using MassTransit.Pipeline;
using System.Threading.Tasks;

namespace Miles.MassTransit.EnsureMessageDispatch
{
    class RecordMessageDispatchFilter<TContext> : IFilter<TContext> where TContext : class, SendContext
    {
        private readonly IDispatchedRepository repository;

        public RecordMessageDispatchFilter(IDispatchedRepository repository)
        {
            this.repository = repository;
        }

        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("record-message-dispatch");
        }

        public async Task Send(TContext context, IPipe<TContext> next)
        {
            await next.Send(context).ConfigureAwait(false);
            await repository.RecordAsync(context).ConfigureAwait(false);
        }
    }
}
