using MassTransit;
using MassTransit.Pipeline;
using System.Threading.Tasks;

namespace Miles.MassTransit.EnsureMessageDispatch
{
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
    /// <summary>
    /// Once a message is dispatched this calls the repository to record the fact.
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <seealso cref="MassTransit.Pipeline.IFilter{TContext}" />
    class RecordMessageDispatchFilter<TContext> : IFilter<TContext> where TContext : class, SendContext
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
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
