using Miles.Messaging;
using System;
using System.Reflection;

namespace Miles.MassTransit.Courier
{
    public static class QueueNameGenerateExtensions
    {
        private const string Arguments = "Arguments";
        private const string Logs = "Logs";
        private const string Compensate = "Compensate";

        public static string GenerateExecutionQueueName(this Type type)
        {
            return GenerateQueueName(type, Arguments);
        }

        public static string GenerateCompensationQueueName(this Type type)
        {
            return GenerateQueueName(type, Logs, Compensate);
        }

        private static string GenerateQueueName(Type type, string remove, string append = "")
        {
            string baseQueueName;

            var queueNameAttrib = type.GetCustomAttribute<QueueNameAttribute>();
            if (queueNameAttrib == null)
                baseQueueName = (type.Name.EndsWith(remove) ? type.Name.Substring(0, type.Name.Length - remove.Length) : type.Name) + append;
            else
                baseQueueName = queueNameAttrib.Name;

            return type.GetQueuePrefix() + baseQueueName + type.GetQueueSuffix();
        }
    }
}
