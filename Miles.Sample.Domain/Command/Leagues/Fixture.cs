using Miles.Sample.Domain.Command.Teams;
using System;

namespace Miles.Sample.Domain.Command.Leagues
{
    class Fixture
    {
        public enum FixtureStates
        {
            Scheduled = 1,
            InProgress = 2,
            Finished = 3
        }

        public Guid Id { get; private set; }

        public LeagueAbbreviation League { get; private set; }

        public TeamAbbreviation TeamA { get; private set; }

        public int TeamAPoints { get; private set; } = 0;

        public TeamAbbreviation TeamB { get; private set; }

        public int TeamBPoints { get; private set; } = 0;

        public FixtureStates State { get; private set; } = FixtureStates.Scheduled;

        public DateTime ScheduledDateTime { get; private set; }

        public DateTime? Started { get; private set; }

        public DateTime? Finished { get; private set; }
    }
}
