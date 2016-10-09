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
