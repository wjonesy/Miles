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
using Miles.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorMessageConvention<TProcessor> : IConsumerMessageConvention
        where TProcessor : class
    {
        public IEnumerable<IMessageInterfaceType> GetMessageTypes()
        {
            if (typeof(TProcessor).IsMessageProcessor())
            {
                var interfaceType = CreateInterfaceType(typeof(TProcessor).GetGenericArguments()[0]);
                if (interfaceType.MessageType.IsValueType == false && interfaceType.MessageType != typeof(string))
                    yield return interfaceType;
            }

            IEnumerable<IMessageInterfaceType> types = typeof(TProcessor).GetInterfaces()
                .Where(x => x.IsMessageProcessor())
                .Select(x => CreateInterfaceType(x.GetGenericArguments()[0]))
                .Where(x => x.MessageType.IsValueType == false && x.MessageType != typeof(string));

            foreach (IMessageInterfaceType type in types)
                yield return type;
        }

        private IMessageInterfaceType CreateInterfaceType(Type messageType)
        {
            return (IMessageInterfaceType)Activator.CreateInstance(typeof(MessageProcessorInterfaceType<,>).MakeGenericType(typeof(TProcessor), messageType));
        }
    }
}
