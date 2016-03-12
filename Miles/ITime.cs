using System;

namespace Miles
{
    /// <summary>
    /// Used to decouple code from DateTime static methods allowing mocking of time.
    /// </summary>
    public interface ITime
    {
        /// <summary>
        /// <see cref="DateTime.Now"/>.
        /// </summary>
        DateTime Now { get; }
    }
}
