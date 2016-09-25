using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit.EnsureMessageDispatch
{
    public interface IDispatchedRepository
    {
        Task RecordAsync(SendContext context);

        Task DeleteOldRecordsAsync();
    }
}