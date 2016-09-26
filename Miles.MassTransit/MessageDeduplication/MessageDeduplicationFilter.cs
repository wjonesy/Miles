using MassTransit;
using MassTransit.Pipeline;
using Microsoft.Practices.ServiceLocation;
using Miles.Persistence;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDeduplication
{
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute
#pragma warning disable CS1658 // Warning is overriding an error
    /// <summary>
    /// Wraps message in a <see cref="ITransactionContext"/> in which the message is recorded to ensure it is processed only once.
    /// On identifying a message as already processed the message is removed from the queue without doing any work.
    /// </summary>
    /// <remarks>
    /// This assumes a container will have registered itself as an <see cref="IServiceLocator"/> payload to 
    /// retrieve an <see cref="IConsumedRepository"/> instance that will work with the <see cref="ITransactionContext"/>.
    /// </remarks>
    /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
    /// <seealso cref="MassTransit.Pipeline.IFilter{MassTransit.ConsumerConsumeContext{TConsumer}}" />
    class MessageDeduplicationFilter<TConsumer> : IFilter<ConsumerConsumeContext<TConsumer>> where TConsumer : class, IConsumer
#pragma warning restore CS1658 // Warning is overriding an error
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute
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
