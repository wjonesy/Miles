﻿/*
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
using Microsoft.Practices.Unity.InterceptionExtension;
using Miles.Messaging;
using Miles.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Miles.MassTransit.Unity
{
    class PreventMultipleExecutionRule : IMatchingRule
    {
        public bool Matches(MethodBase member)
        {
            if (member.Name != "ProcessAsync")
                return false;

            // Not the interface itself, but the actual processor implementation
            var declaringType = member.DeclaringType;
            if (declaringType.IsInterface || !declaringType.GetInterfaces().Any(x => x.IsMessageProcessor()))
                return false;

            var preventMultipleExecAttrib = declaringType.GetCustomAttribute<PreventMultipleExecution>();
            return preventMultipleExecAttrib?.Prevent ?? true;
        }
    }
}