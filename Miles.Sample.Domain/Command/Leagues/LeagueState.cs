using Miles.Sample.Domain.Command.Teams;
using System.Collections.Generic;

namespace Miles.Sample.Domain.Command.Leagues
{
    class LeagueState
    {
        public LeagueAbbreviation Id { get; private set; }

        public string Name { get; private set; }
        public ICollection<TeamAbbreviation> RegisteredTeams { get; private set; }
        public virtual ICollection<LeagueStanding> Standings { get; private set; }
    }
}
