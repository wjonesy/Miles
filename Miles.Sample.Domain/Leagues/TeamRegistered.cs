using Miles.Sample.Domain.Teams;

namespace Miles.Sample.Domain.Leagues
{
    class TeamRegistered
    {
        public LeagueAbbreviation Id { get; set; }
        public TeamAbbreviation Team { get; set; }
    }
}