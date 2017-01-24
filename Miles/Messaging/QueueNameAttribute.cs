using System;

namespace Miles.Messaging
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QueueNameAttribute : Attribute
    {
        public QueueNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
