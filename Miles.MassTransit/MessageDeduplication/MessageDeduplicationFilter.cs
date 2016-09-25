using MassTransit;
using MassTransit.Pipeline;
using Microsoft.Practices.ServiceLocation;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDeduplication
{
    class MessageDeduplicationFilter<TConsumer> : IFilter<ConsumerConsumeContext<TConsumer>> where TConsumer : class, IConsumer
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("message-deduplication");
        }

        public async Task Send(ConsumerConsumeContext<TConsumer> context, IPipe<ConsumerConsumeContext<TConsumer>> next)
        {
            var container = context.GetPayload<IServiceLocator>();
            var repository = container.GetInstance<IConsumedRepository>();

            var alreadyProcessed = await repository.RecordAsync(context).ConfigureAwait(false);
            if (!alreadyProcessed)
                await next.Send(context).ConfigureAwait(false);
        }
    }
}
