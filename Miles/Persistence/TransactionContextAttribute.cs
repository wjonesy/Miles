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
using Miles.Persistence;
using System;
using System.Data;

namespace Miles.Messaging
{
    /// <summary>
    /// Attribute to configure the use of <see cref="ITransactionContext"/>.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public class TransactionContextAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether the method/class/assembly classes should be wrapped in
        /// transaction contexts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets that sets a hint for what level of isolation the transaction context should use.
        /// This is only a hint since implementations might not have a concept of isolation levels or 
        /// only the first transaction in the context governs the isolation level so the rest follow.
        /// </summary>
        /// <value>
        /// The hint isolation level.
        /// </value>
        public IsolationLevel? HintIsolationLevel { get; set; }
    }
}
