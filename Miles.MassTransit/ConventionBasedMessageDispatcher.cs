using MassTransit;
using System;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Dispatches messages based on the contract type.
    /// </summary>
    /// <remarks>Uses MassTransit's Publish method.</remarks>
    /// <seealso cref="Miles.MassTransit.IMessageDispatcher" />
    public class ConventionBasedMessageDispatcher : IMessageDispatcher
    {
        private readonly IPublishEndpoint publishEndpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionBasedMessageDispatcher" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="publishEndpoint">The publish endpoint.</param>
        public ConventionBasedMessageDispatcher(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        /// <summary>
        /// Dispatches the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns></returns>
        public Task DispatchAsync(object message, Guid messageId)
        {
            return publishEndpoint.Publish(message, c => c.MessageId = messageId);
        }
    }
}
