using Miles.Sample.Domain.Command.Leagues;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Configurations.Domain.Command.Leagues
{
    class LeagueAbbreviationConfiguration : ComplexTypeConfiguration<LeagueAbbreviation>
    {
        public LeagueAbbreviationConfiguration()
        {
            Property(x => x.Abbreviation).HasMaxLength(6).IsRequired();
        }
    }
}
