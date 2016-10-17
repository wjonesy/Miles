using Microsoft.Practices.ServiceLocation;
using Miles.MassTransit.MessageDeduplication;
using Miles.Messaging;
using Miles.Persistence;
using System;

namespace Miles.MassTransit.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageProcessorsConfigurator
    {
        /// <summary>
        /// Encapsulates the pipe behavior in a <see cref="ITransactionContext" />.
        /// 
        /// Sets defaults for supplied processors.
        /// </summary>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        IMessageProcessorsConfigurator UseTransactionContext(Action<ITransactionContextConfigurator> configure = null);

        /// <summary>
        /// The message is recorded to ensure it is processed only once.
        /// On identifying a message as already processed the message is removed from the queue without doing any work.
        /// This should be wrapped in an <see cref="ITransactionContext"/> to ensure the processing and recording
        /// of the message are a single unit of work.
        /// 
        /// Sets defaults for supplied processors.
        /// </summary>
        /// <remarks>
        /// This assumes a container will have registered itself as an <see cref="IServiceLocator"/> payload to 
        /// retrieve an <see cref="IConsumedRepository"/> instance that will work with the <see cref="ITransactionContext"/>.
        /// </remarks>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        IMessageProcessorsConfigurator UseMessageDeduplication(Action<IMessageDeduplicationConfigurator> configure = null);

        /// <summary>
        /// Configure a message processor, such as adding middleware to the pipeline for the message processor type.
        /// </summary>
        /// <typeparam name="TProcessor">The message processor type</typeparam>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        IMessageProcessorsConfigurator ConfigureProcessor<TProcessor>(Action<IMessageProcessorConfigurator<TProcessor>> configure) where TProcessor : class, IMessageProcessor;
    }
}