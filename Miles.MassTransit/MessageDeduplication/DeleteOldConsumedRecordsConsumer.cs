using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDeduplication
{
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
