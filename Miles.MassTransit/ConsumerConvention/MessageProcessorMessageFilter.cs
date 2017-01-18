// Copyright 2007-2016 Adam Burton, Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
using GreenPipes;
using MassTransit;
using MassTransit.Pipeline;
using MassTransit.Util;
using Miles.Messaging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorMessageFilter<TProcessor, TMessage> : IConsumerMessageFilter<TProcessor, TMessage>
        where TProcessor : class, IMessageProcessor<TMessage>
        where TMessage : class
    {
        void IProbeSite.Probe(ProbeContext context)
        {
            var scope = context.CreateScope("miles-message-processor");
            scope.Add("method", $"ProcessAsync({TypeMetadataCache<TMessage>.ShortName} message)");
        }

        [DebuggerNonUserCode]
        Task IFilter<ConsumerConsumeContext<TProcessor, TMessage>>.Send(
            ConsumerConsumeContext<TProcessor, TMessage> context,
            IPipe<ConsumerConsumeContext<TProcessor, TMessage>> next)
        {
            var messageProcessor = context.Consumer as IMessageProcessor<TMessage>;
            if (messageProcessor == null)
            {
                string message = $"Message Processor type {TypeMetadataCache<TProcessor>.ShortName} is not a consumer of message type {TypeMetadataCache<TMessage>.ShortName}";

                throw new ConsumerMessageException(message);
            }

            return messageProcessor.ProcessAsync(context.Message);
        }
    }
}