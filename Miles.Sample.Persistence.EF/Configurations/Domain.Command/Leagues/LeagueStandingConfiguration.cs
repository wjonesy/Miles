using Miles.Sample.Domain.Command.Leagues;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miles.Sample.Persistence.EF.Configurations.Domain.Command.Leagues
{
    class LeagueStandingConfiguration : EntityTypeConfiguration<LeagueStanding>
    {
        public LeagueStandingConfiguration()
        {
            HasRequired(x => x.League).WithMany(x => x.Standings);
        }
    }
}
