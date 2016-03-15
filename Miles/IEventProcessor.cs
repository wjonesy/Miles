using System.Threading.Tasks;

namespace Miles
{
    public interface IEventProcessor<in TEvent>
    {
        Task ProcessAsync(TEvent evt);
    }
}
