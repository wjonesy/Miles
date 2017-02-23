﻿/*
 *     Copyright 2017 Adam Burton (adz21c@gmail.com)
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
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Miles.EventStore
{
    public class NewtonsoftJsonSerializer<TAggregate> : ISerializer<TAggregate>
    {
        private readonly JsonSerializer serializer;

        public NewtonsoftJsonSerializer(JsonSerializerSettings settings)
        {
            serializer = JsonSerializer.Create(settings);
        }

        public object DeSerialize(RecordedEvent @event)
        {
            using (var ms = new MemoryStream(@event.Data))
            using (var stream = new StreamReader(ms))
            {
                var eventType = Type.GetType(@event.EventType);
                return serializer.Deserialize(stream, eventType);
            }
        }

        public byte[] Serialize(object @event)
        {
            throw new NotImplementedException();
        }
    }
}
