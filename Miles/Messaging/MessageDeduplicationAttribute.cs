/*
 *     Copyright 2016 Adam Burton (adz21c@gmail.com)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;

namespace Miles.Messaging
{
    /// <summary>
    /// Allows message deduplication to be enabled and/or disabled at an assembly or class level.
    /// If not speecified we assume enabled.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = false)]
    public class MessageDeduplicationAttribute : Attribute
    {
        /// <summary>
        /// Gets an instance of the attribute with default values.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static MessageDeduplicationAttribute Default { get; private set; } = new MessageDeduplicationAttribute();

        /// <summary>
        /// Gets or sets a value indicating whether the annotated class or assembly should be prevented from handling an event or command multiple times.
        /// </summary>
        /// <value>
        ///   <c>true</c> if preventing; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; } = true;
    }
}
