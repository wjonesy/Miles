using Miles.Sample.Domain.Command.Teams;

namespace Miles.Sample.Domain.Command.Leagues
{
    class TeamRegistered
    {
        public LeagueAbbreviation Id { get; set; }
        public TeamAbbreviation Team { get; set; }
    }
}