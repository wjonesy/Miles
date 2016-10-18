// Copyright 2007-2016 Adam Burton, Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.using System;

using MassTransit;
using MassTransit.Configurators;
using MassTransit.Internals.Extensions;
using MassTransit.PipeBuilders;
using MassTransit.PipeConfigurators;
using MassTransit.Util;
using Miles.Messaging;
using System.Collections.Generic;

namespace Miles.MassTransit.ConsumerConvention
{
    class SpecificationProxy<TProcessor, TMessage> : IPipeSpecification<ConsumerConsumeContext<TProcessor>>
        where TProcessor : class, IMessageProcessor
        where TMessage : class
    {
        private readonly IPipeSpecification<ConsumeContext<TMessage>> _specification;

        public SpecificationProxy(IPipeSpecification<ConsumeContext<TMessage>> specification)
        {
            _specification = specification;
        }

        public void Apply(IPipeBuilder<ConsumerConsumeContext<TProcessor>> builder)
        {
            var messageBuilder = builder as IPipeBuilder<ConsumeContext<TMessage>>;

            if (messageBuilder != null)
                _specification.Apply(messageBuilder);
        }

        public IEnumerable<ValidationResult> Validate()
        {
            if (!typeof(TProcessor).HasInterface<IMessageProcessor<TMessage>>())
                yield return this.Failure("MessageType", $"is not consumed by {TypeMetadataCache<TProcessor>.ShortName}");

            foreach (var validationResult in _specification.Validate())
            {
                yield return validationResult;
            }
        }
    }
}
