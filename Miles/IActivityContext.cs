using System;

namespace Miles
{
    public interface IActivityContext
    {
        /// <summary>
        /// Gets the correlation identifier. Represents an Id to correlate activity across multiple events.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        Guid CorrelationId { get; }
    }
}
