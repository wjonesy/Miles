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
        /// Gets the message deduplication configuration for the method.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="fallback">if set to <c>true</c> then falls back to class level attribute.</param>
        /// <returns></returns>
        public static MessageDeduplicationAttribute GetMessageDeduplicationConfig(this MethodInfo methodInfo, bool fallback = true)
        {
            var methodAttrib = methodInfo.GetCustomAttribute<MessageDeduplicationAttribute>();
            if (methodAttrib != null)
                return methodAttrib;

            if (fallback)
                return methodInfo.DeclaringType.GetMessageDeduplicationConfig();

            return null;
        }

        /// <summary>
        /// Gets the message deduplication configuration for the class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fallback">if set to <c>true</c> then falls back to assembly level attribute.</param>
        /// <returns></returns>
        public static MessageDeduplicationAttribute GetMessageDeduplicationConfig(this Type type, bool fallback = true)
        {
            var typeAttrib = type.GetCustomAttribute<MessageDeduplicationAttribute>();
            if (typeAttrib != null)
                return typeAttrib;

            if (fallback)
                return type.Assembly.GetMessageDeduplicationConfig();

            return null;
        }

        /// <summary>
        /// Gets the message deduplication configuration for the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static MessageDeduplicationAttribute GetMessageDeduplicationConfig(this Assembly assembly)
        {
            var assemblyAttrib = assembly.GetCustomAttribute<MessageDeduplicationAttribute>();
            if (assemblyAttrib != null)
                return assemblyAttrib;

            return null;
        }
    }
}
