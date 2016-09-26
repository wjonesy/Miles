using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDeduplication
{
    /// <summary>
    /// Built in consumer to perform regular clean up of old messages.
    /// </summary>
    /// <remarks>
    /// This doesn't need to mean deleting, it could mean archiving. The aim is to keep the data lean for fast processing.
    /// </remarks>
    class DeleteOldConsumedRecordsConsumer : IConsumer<IDeleteOldConsumedRecordsCommand>
    {
        private readonly IConsumedRepository consumedRepository;

        public DeleteOldConsumedRecordsConsumer(IConsumedRepository consumedRepository)
        {
            this.consumedRepository = consumedRepository;
        }

        public Task Consume(ConsumeContext<IDeleteOldConsumedRecordsCommand> context)
        {
            return consumedRepository.DeleteOldRecordsAsync();
        }
    }
}
