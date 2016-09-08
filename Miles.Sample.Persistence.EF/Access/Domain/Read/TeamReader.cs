using Miles.Sample.Domain.Read.Leagues;
using Miles.Sample.Domain.Read.Teams;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Access.Domain.Read
{
    public class TeamReader : ITeamReader
    {
        private readonly SampleDbContext dbContext;

        public TeamReader(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public async Task<List<string>> GetTeamsNotInLeagueAsync(string id)
        {
            var teamsInLeague = dbContext.Set<Sample.Domain.Command.Leagues.LeagueStanding>()
                .Where(x => x.League.Abbreviation.Abbreviation == id)
                .Select(x => x.Team.Abbreviation);

            return await dbContext.Teams
                .Where(x => !teamsInLeague.Contains(x.Abbreviation.Abbreviation))
                .Select(x => x.Abbreviation.Abbreviation)
                .OrderBy(x => x)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
