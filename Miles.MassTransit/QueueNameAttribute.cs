using System;

namespace Miles.MassTransit
{
    public class QueueNameAttribute : Attribute
    {
        public QueueNameAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
