using System;
using System.Reflection;

namespace Miles.Messaging
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QueueAffixesAttribute : Attribute
    {
        public string Prefix { get; set; }

        public string Suffix { get; set; }
    }

    public static class QueueAffixReflectionExtensions
    {
        public static string GetQueuePrefix(this Type type)
        {
            var attrib = type.GetCustomAttribute<QueueAffixesAttribute>();
            return attrib?.Prefix ?? type.Assembly.GetQueuePrefix();
        }

        public static string GetQueuePrefix(this Assembly type)
        {
            var attrib = type.GetCustomAttribute<QueueAffixesAttribute>();
            return attrib?.Prefix ?? string.Empty;
        }

        public static string GetQueueSuffix(this Type type)
        {
            var attrib = type.GetCustomAttribute<QueueAffixesAttribute>();
            return attrib?.Suffix ?? type.Assembly.GetQueueSuffix();
        }

        public static string GetQueueSuffix(this Assembly assembly)
        {
            var attrib = assembly.GetCustomAttribute<QueueAffixesAttribute>();
            return attrib?.Suffix ?? string.Empty;
        }
    }
}
