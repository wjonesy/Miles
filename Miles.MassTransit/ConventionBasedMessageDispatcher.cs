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
        /// Initializes a new instance of the <see cref="ConventionBasedMessageDispatcher"/> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="consumeContext">The consume context.</param>
        public ConventionBasedMessageDispatcher(IBus bus, ConsumeContext consumeContext)
        {
            // If we are working off the back of something else we have a consumeContext.
            // If we are initiating action we fallback to the bus
            publishEndpoint = (IPublishEndpoint)consumeContext ?? bus;
        }

        /// <summary>
        /// Dispatches the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns></returns>
        public async Task DispatchAsync(object message, Guid messageId)
        {
            await publishEndpoint.Publish(message, c => c.MessageId = messageId);
        }
    }
}
