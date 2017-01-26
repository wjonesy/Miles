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
using Miles.MassTransit.RecordMessageDispatch;
using System;

namespace MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public static class RecordMessageDispatchExtensions
    {
        /// <summary>
        /// Registers a filter on send pipes to attempt to record the dispatch of any message.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        public static void UseRecordMessageDispatch<TConfigurator>(this TConfigurator configurator, Action<IRecordMessageDispatchConfigurator> configure)
            where TConfigurator : IPublishPipelineConfigurator, ISendPipelineConfigurator
        {
            var spec = new RecordMessageDispatchSpecification<SendContext>();
            configure.Invoke(spec);

            configurator.ConfigureSend(s => s.AddPipeSpecification(spec));
            configurator.ConfigurePublish(p => p.AddPipeSpecification(spec));
        }

        /// <summary>
        /// Registers a filter on send pipes to attempt to record the dispatch of any message.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        public static void UseRecordMessageDispatch(this ISendPipeConfigurator configurator, Action<IRecordMessageDispatchConfigurator> configure)
        {
            var spec = new RecordMessageDispatchSpecification<SendContext>();
            configure.Invoke(spec);

            configurator.AddPipeSpecification(spec);
        }

        /// <summary>
        /// Registers a filter on send pipes to attempt to record the dispatch of <typeparam name="TMessage" /> messages.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <param name="configure">The callback to configure the message pipeline</param>
        /// <returns></returns>
        public static void UseRecordMessageDispatch<TMessage>(this ISendPipeConfigurator configurator, Action<IRecordMessageDispatchConfigurator> configure)
            where TMessage : class
        {
            var spec = new RecordMessageDispatchSpecification<SendContext<TMessage>>();
            configure.Invoke(spec);

            configurator.AddPipeSpecification(spec);
        }
    }
}
