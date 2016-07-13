﻿/*
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
