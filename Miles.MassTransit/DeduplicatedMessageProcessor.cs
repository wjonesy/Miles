﻿using MassTransit;
using Miles.Messaging;
using Miles.Persistence;
using System.Threading.Tasks;

namespace Miles.MassTransit
{
    /// <summary>
    /// Ensures a message is not processed twice.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <seealso cref="MassTransit.IConsumer{TMessage}" />
    public class DeduplicatedMessageProcessor<TMessage> : IMessageProcessor<TMessage> where TMessage : class
    {
        private readonly IMessageProcessor<TMessage> inner;
        private readonly ConsumeContext context;
        private readonly IIncomingMessageRepository incomingMessageRepo;
        private readonly ITransaction transaction;
        private readonly ITime time;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeduplicatedMessageProcessor{TMessage}"/> class.
        /// </summary>
        /// <param name="inner">The inner instance.</param>
        /// <param name="context">The context.</param>
        /// <param name="incomingMessageRepo">The incoming message repo.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="time">The time.</param>
        public DeduplicatedMessageProcessor(IMessageProcessor<TMessage> inner, ConsumeContext context, IIncomingMessageRepository incomingMessageRepo, ITransaction transaction, ITime time)
        {
            this.inner = inner;
            this.context = context;
            this.incomingMessageRepo = incomingMessageRepo;
            this.transaction = transaction;
            this.time = time;
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public async Task ProcessAsync(TMessage message)
        {
            await transaction.BeginAsync().ConfigureAwait(false);
            try
            {
                // De-duplication
                var incomingMessage = new IncomingMessage(context.MessageId.Value, time.Now);
                var processed = await incomingMessageRepo.RecordAsync(incomingMessage).ConfigureAwait(false);
                if (processed)
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                    return;
                }

                await inner.ProcessAsync(message).ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);
            }
            catch
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
                throw;
            }
        }
    }
}
