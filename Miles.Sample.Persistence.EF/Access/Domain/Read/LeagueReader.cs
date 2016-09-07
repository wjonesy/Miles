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
    }
}
