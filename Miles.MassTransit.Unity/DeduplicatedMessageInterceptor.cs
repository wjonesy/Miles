/*
 *     Copyright 2016 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using MassTransit;
using Microsoft.Practices.Unity.InterceptionExtension;
using Miles.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.MassTransit.Unity
{
    class DeduplicatedMessagehandler : ICallHandler
    {
        private readonly ConsumeContext context;
        private readonly IIncomingMessageRepository incomingMessageRepo;
        private readonly ITransactionContext transactionContext;
        private readonly ITime time;

        public DeduplicatedMessagehandler(ConsumeContext context, IIncomingMessageRepository incomingMessageRepo, ITransactionContext transactionContext, ITime time)
        {
            this.context = context;
            this.incomingMessageRepo = incomingMessageRepo;
            this.transactionContext = transactionContext;
            this.time = time;
        }
        
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            var wrapperTask = DoInvoke(() =>
            {
                var methodReturn = getNext()(input, getNext);
                return (Task)methodReturn.ReturnValue;
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
                {
                    await transaction.CommitAsync().ConfigureAwait(false);
                    return;
                }

                await next().ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);
            }
        }
    }
}
