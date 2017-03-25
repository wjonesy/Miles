using EventStore.ClientAPI;
using Microsoft.Practices.Unity;
using Miles.EventStore;
using Miles.EventStore.NewtonsoftJson;
using Miles.Persistence;
using Miles.Sample.Domain.Leagues;
using Miles.Sample.Domain.Teams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miles.Sample.Infrastructure.Unity
{
    public static class DIConfig
    {
        public static IUnityContainer ConfigureSample(this IUnityContainer container, Func<Type, LifetimeManager> lifetimeManager)
        {
            var connection = EventStoreConnection.Create("ConnectTo=tcp://admin:changeit@localhost:2112");
            connection.ConnectAsync().Wait();
            container.RegisterInstance(connection);
            container.RegisterType(typeof(IRepository<,>), typeof(Repository<,>));
            container.RegisterType(typeof(ISerializer<>), typeof(NewtonsoftJsonSerializer<>));
            container.RegisterType<IStreamIdGenerator, StreamIdGenerator>();
            container.RegisterType<IAggregateManager<Team>, AggregateManager<Team, TeamState>>();
            container.RegisterType<IAggregateEventTypeLookup<Team>, AggregateEventTypeLookup<Team, TeamState>>();
            container.RegisterType<IAggregateManager<League>, AggregateManager<League, LeagueState>>();
            container.RegisterType<IAggregateEventTypeLookup<League>, AggregateEventTypeLookup<League, LeagueState>>();
            container.RegisterType<Application.Read.Teams.ITeamReader, TeamReader>();
            container.RegisterType<Application.Read.Leagues.ILeagueReader, LeagueReader>();

            return container;
        }

        class TeamReader : Application.Read.Teams.ITeamReader
        {
            public Task<List<string>> GetTeamsNotInLeagueAsync(string id)
            {
                return Task.FromResult(new List<string> { "LCFC", "MUFC" });
            }
        }

        class LeagueReader : Application.Read.Leagues.ILeagueReader
        {
            public Task<List<Application.Read.Leagues.Fixture>> GetFixturesAsync(string leagueId)
            {
                return Task.FromResult(new List<Application.Read.Leagues.Fixture>
                {
                    new Application.Read.Leagues.Fixture
                    {
                        Id = Guid.NewGuid(),
                        ScheduledDateTime = DateTime.Now,
                        TeamA = "MUFC",
                        TeamAPoints = 0,
                        TeamB = "LCFC",
                        TeamBPoints = 0,
                        Active = false,
                        Completed = new DateTime?()
                    }
                });
            }

            public Task<List<Application.Read.Leagues.League>> GetLeaguesAsync()
            {
                return Task.FromResult(new List<Application.Read.Leagues.League>
                {
                    new Application.Read.Leagues.League
                    {
                        Abbreviation = "Prem",
                        Name = "Premiership"
                    }
                });
            }

            public Task<List<Application.Read.Leagues.Standing>> GetStandingsAsync(string id)
            {
                return Task.FromResult(new List<Application.Read.Leagues.Standing>
                {
                    new Application.Read.Leagues.Standing
                    {
                        Draws = 0,
                        Losses = 0,
                        Played = 0,
                        Name = "MUFC",
                        Points = 0,
                        PointsAgainst = 0,
                        PointsFor = 0,
                        Wins = 0
                    },
                    new Application.Read.Leagues.Standing
                    {
                        Draws = 0,
                        Losses = 0,
                        Played = 0,
                        Name = "LCFC",
                        Points = 0,
                        PointsAgainst = 0,
                        PointsFor = 0,
                        Wins = 0
                    }
                });
            }

            public Task<List<string>> GetTeamsAsync(string leagueId)
            {
                throw new NotImplementedException();
            }
        }
    }
}
