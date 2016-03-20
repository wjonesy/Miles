using MassTransit;
using System;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Dispatches messages to a specific endpoint by lookuping up the endpoint uri
    /// based on the message type.
    /// </summary>
    /// <remarks>Uses MassTransit's Send method.</remarks>
    /// <seealso cref="Miles.MassTransit.IMessageDispatcher" />
    public class LookupBasedMessageDispatch : IMessageDispatcher
    {
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly ILookupEndpointUri endpointUriLookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupBasedMessageDispatch"/> class.
        /// </summary>
        /// <param name="endpointUriLookup">The endpoint URI lookup.</param>
        /// <param name="bus">The bus.</param>
        /// <param name="consumeContext">The consume context.</param>
        public LookupBasedMessageDispatch(ILookupEndpointUri endpointUriLookup, IBus bus, ConsumeContext consumeContext = null)
        {
            this.endpointUriLookup = endpointUriLookup;
            // If we are working off the back of something else we have a consumeContext.
            // If we are initiating action we fallback to the bus
            this.sendEndpointProvider = (ISendEndpointProvider)consumeContext ?? bus;
        }

        /// <summary>
        /// Dispatches the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns></returns>
        public async Task DispatchAsync(object message, Guid messageId)
        {
            var endpointUri = await endpointUriLookup.LookupAsync(message.GetType());
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(endpointUri);
            await sendEndpoint.Send(message, c => c.MessageId = messageId);
        }
    }
}
