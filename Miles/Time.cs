using System;

namespace Miles
{
    /// <summary>
    /// Default implementation of <see cref="Miles.ITime"/> that sits on top of <see cref="DateTime"/>
    /// </summary>
    /// <seealso cref="Miles.ITime" />
    public class Time : ITime
    {
        /// <summary>
        ///   <see cref="DateTime.Now" />.
        /// </summary>
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}
