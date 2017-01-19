using System;

namespace Miles.Messaging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnoreProcessorAttribute : Attribute
    { }
}
