using System;

namespace Miles.EventStore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class EventAliasAttribute : Attribute
    {
        public EventAliasAttribute(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentNullException(nameof(alias));

            this.Alias = alias;
        }

        public string Alias { get; private set; }
    }
}
