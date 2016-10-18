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
using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.TransactionContext;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorOptions
    {
        public TransactionContextConfigurator TransactionContext { get; set; }
        public MessageDeduplicationConfigurator MessageDeduplication { get; set; }

        public MessageProcessorOptions Merge(MessageProcessorOptions defaults)
        {
            return new MessageProcessorOptions
            {
                TransactionContext = TransactionContext ?? defaults.TransactionContext,
                MessageDeduplication = MessageDeduplication ?? defaults.MessageDeduplication
            };
        }
    }
}
