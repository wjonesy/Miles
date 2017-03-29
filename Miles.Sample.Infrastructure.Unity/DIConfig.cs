using EventStore.ClientAPI;
using Microsoft.Practices.Unity;
using Miles.EventStore;
using Miles.EventStore.NewtonsoftJson;
using Miles.Persistence;
using Miles.Sample.Domain.Leagues;
using Miles.Sample.Domain.Teams;
using Miles.Sample.WebReadStore.Adapters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.Sample.Infrastructure.Unity
{
    public static class DIConfig
    {
        public static IUnityContainer ConfigureSample(this IUnityContainer container, Func<Type, LifetimeManager> lifetimeManager)
        {
            var connection = EventStoreConnection.Create("ConnectTo=tcp://admin:changeit@localhost:1113");
            connection.ConnectAsync().Wait();
            container.RegisterInstance(connection, new ContainerControlledLifetimeManager());
            container.RegisterType(typeof(IRepository<,>), typeof(Repository<,>));
            container.RegisterType(typeof(ISerializer<>), typeof(NewtonsoftJsonSerializer<>));
            container.RegisterType<IStreamIdGenerator, StreamIdGenerator>();
            container.RegisterType<IAggregateManager<Team, TeamAbbreviation>, AggregateManager<Team, TeamAbbreviation, TeamState>>();
            container.RegisterType<IAggregateEventTypeLookup<Team>, AggregateEventTypeLookup<Team, TeamState>>();
            container.RegisterType<IAggregateManager<League, LeagueAbbreviation>, AggregateManager<League, LeagueAbbreviation, LeagueState>>();
            container.RegisterType<IAggregateEventTypeLookup<League>, AggregateEventTypeLookup<League, LeagueState>>();
            container.RegisterType<Application.Read.Teams.ITeamReader, TeamReader>();
            container.RegisterType<Application.Read.Leagues.ILeagueReader, LeagueReader>(lifetimeManager(typeof(LeagueReader)));

            return container;
        }

        class TeamReader : Application.Read.Teams.ITeamReader
        {
            public Task<List<string>> GetTeamsNotInLeagueAsync(string id)
            {
                return Task.FromResult(new List<string> { "LCFC", "MUFC" });
            }
        }
    }
}
