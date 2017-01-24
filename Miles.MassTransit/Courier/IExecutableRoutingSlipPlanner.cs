using System.Threading.Tasks;

namespace Miles.MassTransit.Courier
{
    public interface IExecutableRoutingSlipPlanner : IRoutingSlipPlanner
    {
        Task Execute();
    }
}
