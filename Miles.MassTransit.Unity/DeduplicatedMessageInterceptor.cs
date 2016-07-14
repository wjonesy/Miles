using MassTransit;
using Microsoft.Practices.Unity.InterceptionExtension;
using Miles.Messaging;
using Miles.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.MassTransit.Unity
{
    class DeduplicatedMessageInterceptor : IInterceptionBehavior
    {
        private readonly ConsumeContext context;
        private readonly IIncomingMessageRepository incomingMessageRepo;
        private readonly ITransactionContext transactionContext;
        private readonly ITime time;

        public DeduplicatedMessageInterceptor(ConsumeContext context, IIncomingMessageRepository incomingMessageRepo, ITransactionContext transactionContext, ITime time)
        {
            this.context = context;
            this.incomingMessageRepo = incomingMessageRepo;
            this.transactionContext = transactionContext;
            this.time = time;
        }

        public bool WillExecute
        {
            get { return true; }
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return new[] { typeof(IMessageProcessor<>) };
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var wrapperTask = DoInvoke(() =>
            {
                var methodReturn = getNext()(input, getNext);
                var innerTask = (Task)methodReturn.ReturnValue;
                return innerTask;
            });
            return input.CreateMethodReturn(wrapperTask);
        }

        private async Task DoInvoke(Func<Task> next)
        {
            using (var transaction = await transactionContext.BeginAsync().ConfigureAwait(false))
            {
                // De-duplication
                var incomingMessage = new IncomingMessage(context.MessageId.Value, time.Now);
                var processed = await incomingMessageRepo.RecordAsync(incomingMessage).ConfigureAwait(false);
                if (processed)
                    return;

                await next().ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);
            }
        }
    }
}
