using System;

namespace Miles.Messaging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueueNameAttribute : Attribute
    {
        public QueueNameAttribute(string queueName)
        {
            this.QueueName = queueName;
        }

        public string QueueName { get; private set; }
    }
}
