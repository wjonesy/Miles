using MassTransit;
using System;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Dispatches messages to a specific endpoint by looking up the endpoint uri
    /// based on the message type.
    /// </summary>
    /// <remarks>Uses MassTransit's Send method.</remarks>
    /// <seealso cref="Miles.MassTransit.IMessageDispatcher" />
    public class LookupBasedMessageDispatch : IMessageDispatcher
    {
        private readonly ISendEndpointProvider sendEndpointProvider;
        private readonly ILookupEndpointUri endpointUriLookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupBasedMessageDispatch" /> class.
        /// </summary>
        /// <param name="endpointUriLookup">The endpoint URI lookup.</param>
        /// <param name="sendEndpointProvider">The send endpoint provider.</param>
        public LookupBasedMessageDispatch(ILookupEndpointUri endpointUriLookup, ISendEndpointProvider sendEndpointProvider)
        {
            this.endpointUriLookup = endpointUriLookup;
            this.sendEndpointProvider = sendEndpointProvider;
        }

        /// <summary>
        /// Dispatches the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <returns></returns>
        public async Task DispatchAsync(object message, Guid messageId)
        {
            var endpointUri = await endpointUriLookup.LookupAsync(message.GetType()).ConfigureAwait(false);
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(endpointUri).ConfigureAwait(false);
            await sendEndpoint.Send(message, c => c.MessageId = messageId).ConfigureAwait(false);
        }
    }
}
