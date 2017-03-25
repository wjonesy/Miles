using EventStore.ClientAPI;
using Microsoft.Practices.Unity;
using Miles.EventStore;
using Miles.EventStore.NewtonsoftJson;
using Miles.Persistence;
using Miles.Sample.Domain.Leagues;
using Miles.Sample.Domain.Teams;
using System;

namespace Miles.Sample.Infrastructure.Unity
{
    public static class DIConfig
    {
        public static IUnityContainer ConfigureSample(this IUnityContainer container, Func<Type, LifetimeManager> lifetimeManager)
        {
            var connection = EventStoreConnection.Create("tcp://admin:changeit@localhost:2112");
            connection.ConnectAsync().Wait();
            container.RegisterInstance(connection);
            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));
            container.RegisterType(typeof(ISerializer<>), typeof(NewtonsoftJsonSerializer<>));
            container.RegisterType<IStreamIdGenerator, StreamIdGenerator>();
            container.RegisterType<IAggregateManager<Team>, AggregateManager<Team, TeamState>>();
            container.RegisterType<IAggregateEventTypeLookup<Team>, AggregateEventTypeLookup<Team, TeamState>>();
            container.RegisterType<IAggregateManager<League>, AggregateManager<League, LeagueState>>();
            container.RegisterType<IAggregateEventTypeLookup<League>, AggregateEventTypeLookup<League, LeagueState>>();

            return container;
        }
    }
}
