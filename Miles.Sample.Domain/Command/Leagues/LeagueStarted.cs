using System;

namespace Miles.Sample.Domain.Command.Leagues
{
    class LeagueStarted
    {
        public LeagueAbbreviation Id { get; set; }
        public DateTime When { get; set; }
    }
}