using System;

namespace Miles.Sample.Domain.Command.Leagues
{
    class FixtureStarted
    {
        public Guid Id { get; set; }

        public LeagueAbbreviation League { get; set; }

        public DateTime When { get; set; }
    }
}