using MassTransit;
using Miles.MassTransit.EnsureMessageDispatch;
using System;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class RecordMessageDispatchExtensions
    {
        /// <summary>
        /// Registers a filter on send pipes to attempt to record the dispatch of any message.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="configure">The configuration.</param>
        /// <returns></returns>
        public static ISendPipeConfigurator UseRecordMessageDispatch(this ISendPipeConfigurator configurator, Action<IRecordMessageDispatchConfigurator> configure)
        {
            var spec = new RecordMessageDispatchSpecification<SendContext>();
            configure.Invoke(spec);

            configurator.AddPipeSpecification(spec);
            return configurator;
        }

        /// <summary>
        /// Registers a filter on send pipes to attempt to record the dispatch of <typeparam name="TMessage" /> messages.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="configure">The configuration.</param>
        /// <returns></returns>
        public static ISendPipeConfigurator UseRecordMessageDispatch<TMessage>(this ISendPipeConfigurator configurator, Action<IRecordMessageDispatchConfigurator> configure)
            where TMessage : class
        {
            var spec = new RecordMessageDispatchSpecification<SendContext<TMessage>>();
            configure.Invoke(spec);

            configurator.AddPipeSpecification(spec);
            return configurator;
        }
    }
}
