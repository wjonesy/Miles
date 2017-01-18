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
using GreenPipes;
using MassTransit;
using MassTransit.Configuration;
using MassTransit.ConsumeConfigurators;
using MassTransit.ConsumeConnectors;
using Miles.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.MassTransit.ConsumerConvention
{
    class MessageProcessorConfigurator<TProcessor> : IConsumerConfigurator<TProcessor>, IConsumerConfigurationObserverConnector, IReceiveEndpointSpecification
        where TProcessor : class, IMessageProcessor
    {
        private readonly IConsumerFactory<TProcessor> consumerFactory;
        private readonly IConsumerSpecification<TProcessor> specification;

        public MessageProcessorConfigurator(IConsumerFactory<TProcessor> consumerFactory, IConsumerConfigurationObserver observer)
        {
            this.consumerFactory = consumerFactory;

            specification = ConsumerConnectorCache<TProcessor>.Connector.CreateConsumerSpecification<TProcessor>();

            specification.ConnectConsumerConfigurationObserver(observer);
        }

        public void AddPipeSpecification(IPipeSpecification<ConsumerConsumeContext<TProcessor>> specification)
        {
            this.specification.AddPipeSpecification(specification);
        }

        public ConnectHandle ConnectConsumerConfigurationObserver(IConsumerConfigurationObserver observer)
        {
            return specification.ConnectConsumerConfigurationObserver(observer);
        }

        void IConsumerConfigurator<TProcessor>.ConfigureMessage<T>(Action<IConsumerMessageConfigurator<T>> configure)
        {
            specification.Message(configure);
        }

        void IConsumerConfigurator<TProcessor>.Message<T>(Action<IConsumerMessageConfigurator<T>> configure)
        {
            specification.Message(configure);
        }

        void IConsumerConfigurator<TProcessor>.ConsumerMessage<T>(Action<IConsumerMessageConfigurator<TProcessor, T>> configure)
        {
            specification.ConsumerMessage(configure);
        }

        public IEnumerable<ValidationResult> Validate()
        {
            return consumerFactory.Validate().Concat(specification.Validate());
        }

        public void Configure(IReceiveEndpointBuilder builder)
        {
            ConsumerConnectorCache<TProcessor>.Connector.ConnectConsumer(builder, consumerFactory, specification);
        }
    }
}