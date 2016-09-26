using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit.EnsureMessageDispatch
{
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
