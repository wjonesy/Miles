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
using Miles.MassTransit.Configuration;

namespace Miles.MassTransit.ConsumerConvention
{
    public class MilesConfigurator : IMilesConfigurator
    {
        public string QueueNamePrefix { get; set; }

        //#region Configurator

        //private readonly MessageProcessorOptions options = new MessageProcessorOptions();
        //private readonly Dictionary<Type, IMessageProcessorConfigurator> processorConfigurators = new Dictionary<Type, IMessageProcessorConfigurator>();

        //IMilesConfigurator IMilesConfigurator.UseTransactionContext(Action<ITransactionContextConfigurator> configure)
        //{
        //    options.TransactionContext = new TransactionContextConfigurator();
        //    configure?.Invoke(options.TransactionContext);
        //    return this;
        //}


        //IMilesConfigurator IMilesConfigurator.UseMessageDeduplication(Action<IMessageDeduplicationConfigurator> configure)
        //{
        //    options.MessageDeduplication = new MessageDeduplicationConfigurator();
        //    configure?.Invoke(options.MessageDeduplication);
        //    return this;
        //}

        //IMilesConfigurator IMilesConfigurator.ConfigureProcessor<TProcessor>(Action<IMessageProcessorConfigurator<TProcessor>> configure)
        //{
        //    var configurator = new MessageProcessorConfigurator<TProcessor>();
        //    configure.Invoke(configurator);
        //    processorConfigurators[typeof(TProcessor)] = configurator;
        //    return this;
        //}

        //#endregion

        //public IReceiveEndpointSpecification CreateEndpointSpecification(Type processorType, IConsumerFactoryFactory factory)
        //{
        //    IMessageProcessorConfigurator configurator;
        //    if (!processorConfigurators.TryGetValue(processorType, out configurator))
        //        configurator = (IMessageProcessorConfigurator)Activator.CreateInstance(typeof(MessageProcessorConfigurator<>).MakeGenericType(processorType));
        //    return configurator.CreateSpecification(factory, options);
        //}
    }
}
