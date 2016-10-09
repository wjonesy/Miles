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
using Microsoft.Practices.Unity;
using Miles.MassTransit.MessageDeduplication;
using Miles.MassTransit.MessageDispatch;
using Miles.MassTransit.RecordMessageDispatch;
using Miles.MassTransit.TransactionContext;
using Miles.Messaging;

namespace Miles.MassTransit.Unity
{
    /// <exclude />
    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterMilesMassTransit(this IUnityContainer container, UnityRegistrationConfiguration configuration = null)
        {
            configuration = configuration ?? new UnityRegistrationConfiguration();

            container
                .RegisterType<IActivityContext, ActivityContext>(configuration.ChildContainerLifetimeManagerFactory(typeof(ActivityContext)), new InjectionConstructor(new OptionalParameter<ConsumeContext>()));

            container
                .RegisterType<IEventPublisher, TransactionalMessagePublisher>()
                .RegisterType<ICommandPublisher, TransactionalMessagePublisher>()
                .RegisterType<TransactionalMessagePublisher>(configuration.ChildContainerLifetimeManagerFactory(typeof(TransactionalMessagePublisher)));

            switch (configuration.CommandDispatcher)
            {
                case CommandDispatcherTypes.Lookup:
                    container.RegisterType<IMessageDispatcher, LookupBasedMessageDispatch>(configuration.ChildContainerLifetimeManagerFactory(typeof(LookupBasedMessageDispatch)));
                    break;
                default:
                    container.RegisterType<IMessageDispatcher, ConventionBasedMessageDispatcher>(configuration.ChildContainerLifetimeManagerFactory(typeof(ConventionBasedMessageDispatcher)));
                    break;
            }

            switch (configuration.MessageDispatchProcess)
            {
                case MessageDispatchProcesses.OutOfThread:
                    container.RegisterInstance<IMessageDispatchProcess>(new OutOfThreadMessageDispatchProcess(), new ContainerControlledLifetimeManager());
                    break;
                default:
                    container.RegisterType<IMessageDispatchProcess, ImmediateMessageDispatchProcess>(configuration.ChildContainerLifetimeManagerFactory(typeof(ImmediateMessageDispatchProcess)));
                    break;
            }

            container.RegisterType<IConsumer<IDeleteOldConsumedRecordsCommand>, DeleteOldConsumedRecordsConsumer>(configuration.ChildContainerLifetimeManagerFactory(typeof(DeleteOldConsumedRecordsConsumer)));
            container.RegisterType<IConsumer<IDeleteOldDispatchRecordsCommand>, DeleteOldDispatchRecordsConsumer>(configuration.ChildContainerLifetimeManagerFactory(typeof(DeleteOldDispatchRecordsConsumer)));

            container.RegisterMessageProcessors(configuration.ProcessorTypes, configuration.ChildContainerLifetimeManagerFactory);

            return container;
        }
    }
}
