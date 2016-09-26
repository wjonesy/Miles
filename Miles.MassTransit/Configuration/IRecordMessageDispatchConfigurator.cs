using Miles.MassTransit.EnsureMessageDispatch;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// Allows developer to configure the <see cref="RecordMessageDispatchFilter{TContext}"/> .
    /// </summary>
    public interface IRecordMessageDispatchConfigurator
    {
        /// <summary>
        /// Provide the <see cref="IDispatchedRepository"/> instance for recording message dispatch. 
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <returns></returns>
        IRecordMessageDispatchConfigurator UseDispatchedRepository(IDispatchedRepository repository);
    }
}
