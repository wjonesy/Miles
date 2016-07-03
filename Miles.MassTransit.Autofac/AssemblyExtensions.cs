using Miles.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.Autofac
{
    public static class AssemblyExtensions
    {
        public static bool IsMessageProcessor(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IMessageProcessor<>);
        }

        public static IEnumerable<Type> GetMessageProcessors(this IEnumerable<Type> types)
        {
            return types.Where(x => x.GetInterfaces().Any(i => i.IsMessageProcessor()));
        }

        public static IEnumerable<Type> GetProcessedMessageTypes(this IEnumerable<Type> types)
        {
            return types.Concat(types.SelectMany(x => x.GetInterfaces()))
                .Where(x => x.IsMessageProcessor()).Select(x => x.GetGenericArguments().First());
        }
    }
}
