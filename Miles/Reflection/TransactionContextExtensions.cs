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
    public static class TransactionContextExtensions
    {
        /// <summary>
        /// Gets the transaction context configuration for the method.
        /// Looks at the method first, then the class type and finally the assembly type
        /// before falling back to defaults.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns></returns>
        public static TransactionContextAttribute GetTransactionConfig(this MethodInfo methodInfo)
        {
            var methodAttrib = methodInfo.GetCustomAttribute<TransactionContextAttribute>();
            if (methodAttrib != null)
                return methodAttrib;

            return methodInfo.DeclaringType.GetTransactionConfig();
        }

        /// <summary>
        /// Gets the transaction context configuration for the class.
        /// Looks at the class type and finally the assembly type
        /// before falling back to defaults.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static TransactionContextAttribute GetTransactionConfig(this Type type)
        {
            var typeAttrib = type.GetCustomAttribute<TransactionContextAttribute>();
            if (typeAttrib != null)
                return typeAttrib;

            var assemblyAttrib = type.Assembly.GetCustomAttribute<TransactionContextAttribute>();
            if (assemblyAttrib != null)
                return assemblyAttrib;

            return new TransactionContextAttribute { Enabled = true };
        }
    }
}
