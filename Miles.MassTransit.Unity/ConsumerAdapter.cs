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
using Miles.Messaging;
using System.Threading.Tasks;

namespace Miles.MassTransit.Unity
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
