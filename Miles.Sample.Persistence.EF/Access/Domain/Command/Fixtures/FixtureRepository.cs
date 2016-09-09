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

        public Task<Fixture> GetByIdAsync(FixtureId fixtureId)
        {
            return dbContext.Fixtures.SingleOrDefaultAsync(x => x.Id.Id == fixtureId.Id);
        }

        public Task SaveAsync(Fixture fixture)
        {
            var entry = dbContext.Entry(fixture);
            if (entry == null || entry.State == EntityState.Detached || entry.State == EntityState.Deleted)
                dbContext.Fixtures.Add(fixture);
            return dbContext.SaveChangesAsync();
        }
    }
}
