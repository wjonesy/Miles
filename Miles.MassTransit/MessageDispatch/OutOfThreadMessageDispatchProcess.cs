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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.MassTransit.MessageDispatch
{
    /// <summary>
    /// Dispatches messages in a separate thread to allow processing code to complete earlier.
    /// </summary>
    /// <seealso cref="IMessageDispatchProcess" />
    public class OutOfThreadMessageDispatchProcess : IMessageDispatchProcess
    {
        private static readonly Task AlreadyCompleted = Task.FromResult(0);

        private readonly BlockingCollection<OutgoingMessageForDispatch> queue = new BlockingCollection<OutgoingMessageForDispatch>(new ConcurrentQueue<OutgoingMessageForDispatch>());

        private readonly ConventionBasedMessageDispatcher eventDispatcher;
        private readonly IMessageDispatcher commandDispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfThreadMessageDispatchProcess"/> class.
        /// </summary>
        /// <param name="eventDispatcher">The event dispatcher.</param>
        /// <param name="commandDispatcher">The command dispatcher.</param>
        public OutOfThreadMessageDispatchProcess(
            ConventionBasedMessageDispatcher eventDispatcher,
            IMessageDispatcher commandDispatcher)
        {
            this.eventDispatcher = eventDispatcher;
            this.commandDispatcher = commandDispatcher;

            var t = Task.Run(async () =>
            {
                var message = queue.Take();
                while (message != null)
                {
                    var dispatcher = message.ConceptType == OutgoingMessageConceptType.Command ? commandDispatcher : eventDispatcher;
                    await dispatcher.DispatchAsync(message);

                    message = queue.Take();
                }
            });
        }

        /// <summary>
        /// Initiates the dispatch of messages to the message queue
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public Task ExecuteAsync(IEnumerable<OutgoingMessageForDispatch> messages)
        {
            foreach (var message in messages)
                queue.Add(message);

            return AlreadyCompleted;
        }
    }
}
