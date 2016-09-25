using MassTransit;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDeduplication
{
    public interface IConsumedRepository
    {
        Task<bool> RecordAsync(MessageContext messageContext);

        Task DeleteOldRecordsAsync();
    }
}