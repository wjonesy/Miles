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
    public static class MessageDeduplicationExtensions
    {
        /// <summary>
        /// Determines whether the message processor is interested in message deduplication.
        /// Looks at the class type first and works up to the assembly type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsMessageDeduplicationEnabled(this Type type)
        {
            var typeAttrib = type.GetCustomAttribute<MessageDeduplicationAttribute>();
            if (typeAttrib != null)
                return typeAttrib.Enabled;

            var assemblyAttrib = type.Assembly.GetCustomAttribute<MessageDeduplicationAttribute>();
            if (assemblyAttrib != null)
                return assemblyAttrib.Enabled;

            return true;
        }
    }
}
