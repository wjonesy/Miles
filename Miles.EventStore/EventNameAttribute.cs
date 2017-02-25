using System;

namespace Miles.EventStore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EventNameAttribute : Attribute
    {
        public EventNameAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
