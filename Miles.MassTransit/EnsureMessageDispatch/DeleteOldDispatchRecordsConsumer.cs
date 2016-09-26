using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit.EnsureMessageDispatch
{
    /// <summary>
    /// Built in consumer to perform regular clean up of old messages.
    /// </summary>
    /// <remarks>
    /// This doesn't need to mean deleting, it could mean archiving. The aim is to keep the data lean for fast processing.
    /// </remarks>
    class DeleteOldDispatchRecordsConsumer : IConsumer<IDeleteOldDispatchRecordsCommand>
    {
        private readonly IDispatchedRepository dispatchedRepository;

        public DeleteOldDispatchRecordsConsumer(IDispatchedRepository dispatchedRepository)
        {
            this.dispatchedRepository = dispatchedRepository;
        }

        public Task Consume(ConsumeContext<IDeleteOldDispatchRecordsCommand> context)
        {
            return dispatchedRepository.DeleteOldRecordsAsync();
        }
    }
}
