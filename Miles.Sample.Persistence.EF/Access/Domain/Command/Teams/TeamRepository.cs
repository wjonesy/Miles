using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Access.Domain.Command.Teams
{
    public class TeamRepository : ITeamRepository
    {
        private readonly SampleDbContext dbContext;

        public TeamRepository(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task SaveAsync(Team team)
        {
            dbContext.Teams.Add(team);
            return dbContext.SaveChangesAsync();
        }
    }
}
