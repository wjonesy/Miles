using Miles.Sample.Domain.Command.Teams;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Configurations.Domain.Command.Teams
{
    class TeamAbbreviationConfiguration : ComplexTypeConfiguration<TeamAbbreviation>
    {
        public TeamAbbreviationConfiguration()
        {
            Property(x => x.Abbreviation).HasMaxLength(6).IsRequired();
        }
    }
}
