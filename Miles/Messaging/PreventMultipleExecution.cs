using System;

namespace Miles.Messaging
{
    /// <summary>
    /// Allows the developer to indicate if we want to prevent the multiple execution
    /// of and event handler
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class PreventMultipleExecution : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the annotated class should be prevented from handling an event or command multiple times.
        /// </summary>
        /// <value>
        ///   <c>true</c> if preventing; otherwise, <c>false</c>.
        /// </value>
        public bool Prevent { get; set; } = true;
    }
}
