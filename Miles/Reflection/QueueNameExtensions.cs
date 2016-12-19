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
using Miles.Messaging;
using System;
using System.Reflection;

namespace Miles.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public static class QueueNameExtensions
    {
        /// <summary>
        /// Gets the transaction context configuration for the class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fallback">if set to <c>true</c> then falls back to assembly level attribute.</param>
        /// <returns></returns>
        public static QueueNameAttribute GetQueueNameConfig(this Type type, bool fallback = true)
        {
            var typeAttrib = type.GetCustomAttribute<QueueNameAttribute>();
            if (typeAttrib != null)
                return typeAttrib;

            if (fallback)
                return new QueueNameAttribute(type.Name);

            return null;
        }
    }
}
