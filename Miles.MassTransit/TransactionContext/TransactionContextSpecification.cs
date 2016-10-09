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
using MassTransit.Configurators;
using MassTransit.PipeBuilders;
using MassTransit.PipeConfigurators;
using Miles.MassTransit.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Miles.MassTransit.TransactionContext
{
    class TransactionContextSpecification<TContext> : IPipeSpecification<TContext>, ITransactionContextConfigurator
        where TContext : class, ConsumeContext
    {
        private IsolationLevel? _hintIsolationLevel;

        public ITransactionContextConfigurator HintIsolationLevel(IsolationLevel? isolationLevel)
        {
            _hintIsolationLevel = isolationLevel;
            return this;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<TContext> builder)
        {
            builder.AddFilter(new TransactionContextFilter<TContext>(_hintIsolationLevel));
        }
    }
}
