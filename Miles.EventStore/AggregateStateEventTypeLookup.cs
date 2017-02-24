using Miles.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Miles.EventStore
{
    class AggregateStateEventTypeLookup<TState> where TState : class, IAppliesEvent
    {
        private static readonly Type iAppliesEvents = typeof(IAppliesEvent<>);

        private readonly Dictionary<string, Type> types;

        public AggregateStateEventTypeLookup()
        {
            var applyableEvents = typeof(TState).GetInterfaces()
                .Where(t => t.IsGenericType)
                .Where(t => t.GetGenericTypeDefinition() == iAppliesEvents)
                .Select(t => t.GetGenericArguments().First());

            var typesAndAliases = applyableEvents
                .SelectMany(et => et.GetCustomAttributes<EventAliasAttribute>(false)
                    .Select(n => new
                    {
                        Alias = n.Alias,
                        Type = et
                    })
                ).ToList();

            var clashingAliasCheck = typesAndAliases.GroupBy(x => x.Alias, x => x.Type).Where(x => x.Count() > 1).ToList();
            if (clashingAliasCheck.Any())
                throw new ClashingEventAliasException(typeof(TState), clashingAliasCheck);

            types = typesAndAliases.ToDictionary(x => x.Alias, x => x.Type);
        }

        public Type this[string alias]
        {
            get
            {
                Type et;
                if (types.TryGetValue(alias, out et))
                    return et;

                return null;
            }
        }
    }
}
