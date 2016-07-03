using MassTransit;
using Miles.Messaging;
using System.Threading.Tasks;

namespace Miles.MassTransit.Autofac
{
    /// <summary>
    /// Adapts the message processor to a MassTransit consumer.
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <seealso cref="MassTransit.IConsumer{TMessage}" />
    class ConsumerAdapter<TMessage> : IConsumer<TMessage> where TMessage : class
    {
        private readonly IMessageProcessor<TMessage> processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerAdapter{TMessage}"/> class.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public ConsumerAdapter(IMessageProcessor<TMessage> processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Consumes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<TMessage> context)
        {
            return processor.ProcessAsync(context.Message);
        }
    }
}
