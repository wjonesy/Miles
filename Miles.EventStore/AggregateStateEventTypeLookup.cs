using Miles.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Miles.EventStore
{
    public class AggregateStateEventTypeLookup<TState> where TState : class, IAppliesEvent
    {
        private static readonly Type iAppliesEvents = typeof(IAppliesEvent<>);

        private readonly Dictionary<string, Type> aliasToType;
        private readonly Dictionary<Type, string> typeToAlias;

        public AggregateStateEventTypeLookup()
        {
            var applyableEvents = typeof(TState).GetInterfaces()
                .Where(t => t.IsGenericType)
                .Where(t => t.GetGenericTypeDefinition() == iAppliesEvents)
                .Select(t => t.GetGenericArguments().First());

            var nameAndType = applyableEvents
                .Select(et => new
                {
                    Alias = et.GetCustomAttribute<EventNameAttribute>(false)?.Name ?? et.Name,
                    Type = et
                })
                .ToList();

            var aliasAndType = applyableEvents
                .SelectMany(et => et.GetCustomAttributes<EventAliasAttribute>(false)
                    .Select(n => new
                    {
                        Alias = n.Alias,
                        Type = et
                    }))
                    .Concat(nameAndType)
                    .ToList();

            var clashingAliasCheck = aliasAndType.GroupBy(x => x.Alias, x => x.Type).Where(x => x.Count() > 1).ToList();
            if (clashingAliasCheck.Any())
                throw new ClashingEventAliasException(typeof(TState), clashingAliasCheck);

            aliasToType = aliasAndType.ToDictionary(x => x.Alias, x => x.Type);
            typeToAlias = nameAndType.ToDictionary(x => x.Type, x => x.Alias);
        }

        public Type this[string alias]
        {
            get
            {
                Type et;
                if (aliasToType.TryGetValue(alias, out et))
                    return et;

                return null;
            }
        }

        public string this[Type eventType]
        {
            get
            {
                string alias;
                if (typeToAlias.TryGetValue(eventType, out alias))
                    return alias;

                return null;
            }
        }
    }
}
