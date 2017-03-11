﻿using Miles.Sample.Domain.Command.Teams;
using System;

namespace Miles.Sample.Domain.Command.Leagues
{
    public class Fixture
    {
        public Guid Id { get; set; }

        public TeamAbbreviation TeamA { get; set; }

        public int TeamAPoints { get; set; } = 0;

        public TeamAbbreviation TeamB { get; set; }

        public int TeamBPoints { get; set; } = 0;

        public FixtureStates State { get; set; } = FixtureStates.Scheduled;

        public DateTime ScheduledDateTime { get; set; }

        public DateTime? Started { get; set; }

        public DateTime? Finished { get; set; }
    }
}
