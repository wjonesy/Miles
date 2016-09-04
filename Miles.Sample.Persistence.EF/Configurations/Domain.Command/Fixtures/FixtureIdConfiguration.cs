using Miles.Sample.Domain.Command.Fixtures;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Configurations.Domain.Command.Leagues
{
    class FixtureIdConfiguration : ComplexTypeConfiguration<FixtureId>
    {
        public FixtureIdConfiguration()
        {
            Property(x => x.Id).HasMaxLength(50).IsRequired();
        }
    }
}
