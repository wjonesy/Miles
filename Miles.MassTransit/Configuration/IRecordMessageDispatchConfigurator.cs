using Miles.MassTransit.EnsureMessageDispatch;

namespace Miles.MassTransit.Configuration
{
    public interface IRecordMessageDispatchConfigurator
    {
        IRecordMessageDispatchConfigurator UseDispatchedRepository(IDispatchedRepository repository);
    }
}
