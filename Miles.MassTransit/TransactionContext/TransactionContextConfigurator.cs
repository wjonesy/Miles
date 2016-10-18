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
using MassTransit;
using Miles.MassTransit.Configuration;
using Miles.Messaging;
using System.Data;

namespace Miles.MassTransit.TransactionContext
{
    class TransactionContextConfigurator : ITransactionContextConfigurator
    {
        #region Configurator

        public TransactionContextConfigurator(TransactionContextAttribute attrib = null)
        {
            if (attrib != null)
            {
                Enabled = attrib.Enabled;
                HintIsolationLevel = attrib.HintIsolationLevel;
            }
        }

        public bool Enabled { get; private set; } = true;

        public IsolationLevel? HintIsolationLevel { get; private set; }

        ITransactionContextConfigurator ITransactionContextConfigurator.Enable(bool enable)
        {
            Enabled = enable;
            return this;
        }

        ITransactionContextConfigurator ITransactionContextConfigurator.HintIsolationLevel(IsolationLevel? isolationLevel)
        {
            HintIsolationLevel = isolationLevel;
            return this;
        }

        #endregion

        public TransactionContextSpecification<TContext> CreateSpecification<TContext>()
            where TContext : class, ConsumeContext
        {
            return new TransactionContextSpecification<TContext>(this);
        }
    }
}
