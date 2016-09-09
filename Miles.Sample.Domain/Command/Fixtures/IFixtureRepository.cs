using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Domain.Command.Fixtures
{
    public interface IFixtureRepository
    {
        Task<Fixture> GetByIdAsync(FixtureId fixtureId);

        Task SaveAsync(Fixture fixture);
    }
}
