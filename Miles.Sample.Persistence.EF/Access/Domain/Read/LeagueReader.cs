using Miles.Sample.Domain.Read.Leagues;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Access.Domain.Read
{
    public class LeagueReader : ILeagueReader
    {
        private readonly SampleDbContext dbContext;

        public LeagueReader(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<League>> GetLeaguesAsync()
        {
            return await dbContext.Leagues.Select(x => new League
                {
                    Abbreviation = x.Abbreviation.Abbreviation,
                    Name = x.Name
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Standing>> GetStandingsAsync(string id)
        {
            return await dbContext.Set<Sample.Domain.Command.Leagues.LeagueStanding>()
                .Where(x => x.League.Abbreviation.Abbreviation == id)
                .Select(x => new Standing
                {
                    Name = x.Team.Abbreviation,
                    Played = x.Played,
                    //Wins = x.Wins,
                    //Draws = x.Draws,
                    //Losses = x.Losses,
                    PointsFor = x.PointsFor,
                    PointsAgainst = x.PointsAgainst,
                    Points = x.Points
                })
                .OrderByDescending(x => x.Points)
                .ThenByDescending(x => x.PointsFor)
                .ThenByDescending(x => x.PointsAgainst)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Fixture>> GetFixturesAsync(string leagueId)
        {
            return await dbContext.Fixtures
                   .Where(x => x.League.Abbreviation == leagueId)
                   .Select(x => new Fixture
                   {
                       Id = x.Id.Id,
                       TeamA = x.TeamA.Abbreviation,
                       TeamAPoints = !x.Started.HasValue ? new int?() : x.TeamAPoints,
                       TeamB = x.TeamB.Abbreviation,
                       TeamBPoints = !x.Started.HasValue ? new int?() : x.TeamBPoints,
                       ScheduledDateTime = x.ScheduledDateTime,
                       Active = x.Started.HasValue && !x.Finished.HasValue,
                       Completed = x.Finished
                   })
                   .OrderByDescending(x => x.ScheduledDateTime)
                   .AsNoTracking()
                   .ToListAsync();
        }

        public async Task<List<string>> GetTeamsAsync(string leagueId)
        {
            return await dbContext.Set<Sample.Domain.Command.Leagues.LeagueStanding>()
                .Where(x => x.League.Abbreviation.Abbreviation == leagueId)
                .Select(x => x.Team.Abbreviation)
                .OrderBy(x => x)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
