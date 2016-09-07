using Miles.Sample.Domain.Command.Fixtures;
using Miles.Sample.Domain.Command.Leagues;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Access.Domain.Command.Fixtures
{
    public class FixtureRepository : IFixtureRepository
    {
        private readonly SampleDbContext dbContext;

        public FixtureRepository(SampleDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task SaveAsync(Fixture fixture)
        {
            dbContext.Fixtures.Add(fixture);
            return dbContext.SaveChangesAsync();
        }
    }
}
