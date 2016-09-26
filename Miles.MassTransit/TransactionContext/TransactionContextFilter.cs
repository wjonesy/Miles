using MassTransit;
using MassTransit.Pipeline;
using Microsoft.Practices.ServiceLocation;
using Miles.Persistence;
using System.Threading.Tasks;

namespace Miles.MassTransit.TransactionContext
{
#pragma warning disable CS1584 // XML comment has syntactically incorrect cref attribute
#pragma warning disable CS1658 // Warning is overriding an error
    /// <summary>
    /// Encapsulates a consumer behaviour in a transaction context.
    /// </summary>
    /// <typeparam name="TConsumer">The type of the consumer.</typeparam>
    /// <seealso cref="MassTransit.Pipeline.IFilter{MassTransit.ConsumerConsumeContext{TConsumer}}" />
    class TransactionContextFilter<TConsumer> : IFilter<ConsumerConsumeContext<TConsumer>> where TConsumer : class, IConsumer
#pragma warning restore CS1658 // Warning is overriding an error
#pragma warning restore CS1584 // XML comment has syntactically incorrect cref attribute
    {
        public void Probe(ProbeContext context)
        {
            context.CreateFilterScope("transaction-context");
        }

        public async Task Send(ConsumerConsumeContext<TConsumer> context, IPipe<ConsumerConsumeContext<TConsumer>> next)
        {
            // Retrive container controlled instance
            var container = context.GetPayload<IServiceLocator>();
            var transactionContext = container.GetInstance<ITransactionContext>();

            var transaction = await transactionContext.BeginAsync().ConfigureAwait(false);
            try
            {
                await next.Send(context).ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch
            {
                // Rolling back manually rather than as part of dispose allows us to await
                await transaction.RollbackAsync().ConfigureAwait(false);

                throw;
            }
            finally
            {
                transaction.Dispose();
            }
        }
    }
}
