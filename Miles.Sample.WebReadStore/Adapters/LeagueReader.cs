using Miles.Sample.Application.Read.Leagues;
using Miles.Sample.WebReadStore.EF;
using Miles.Sample.WebReadStore.Schema;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Miles.Sample.WebReadStore.Adapters
{
    public class LeagueReader : ILeagueReader
    {
        private readonly WebReadStoreContext context;

        public LeagueReader(WebReadStoreContext context)
        {
            this.context = context;
        }

        public Task<List<Fixture>> GetFixturesAsync(string leagueId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Application.Read.Leagues.League>> GetLeaguesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Standing>> GetStandingsAsync(string id)
        {
            return context.Set<LeagueStanding>().Where(x => x.League.LeagueId == id)
                .Select(x => new Standing
                {
                    Name = x.TeamName,
                    Played = x.GamesPlayed,
                    Wins = x.GamesWon,
                    Draws = x.GamesDrawn,
                    Losses = x.GamesLost,
                    PointsFor = x.GoalsFor,
                    PointsAgainst = x.GoalsAgainst,
                    Points = x.Points
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<List<string>> GetTeamsAsync(string leagueId)
        {
            throw new NotImplementedException();
        }
    }
}
