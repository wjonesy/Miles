using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Miles.EventStore
{
    [Serializable]
    public class ClashingEventAliasException : Exception
    {
        public ClashingEventAliasException() : base("Event types with clashing aliases identified while processing aggregate state")
        { }

        public ClashingEventAliasException(string message) : base(message)
        { }

        public ClashingEventAliasException(string message, Exception innerException) : base(message, innerException)
        { }

        public ClashingEventAliasException(Type aggregateStateType, List<IGrouping<string, Type>> clashingAliasCheck)
            : base($"Event types with clashing aliases identified while processing aggregate state: {aggregateStateType.FullName}")
        {
            this.AggregateStateType = aggregateStateType;
            this.ClashingAliasCheck = clashingAliasCheck;
        }

        protected ClashingEventAliasException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        public IEnumerable<IGrouping<string, Type>> ClashingAliasCheck { get; private set; }
        public Type AggregateStateType { get; private set; }
    }
}