﻿using Miles.Sample.Domain.Teams;
using System;

namespace Miles.Sample.Domain.Leagues
{
    class FixtureScheduled
    {
        public Guid Id { get; set; }

        public LeagueAbbreviation League { get; set; }

        public TeamAbbreviation TeamA { get; set; }

        public TeamAbbreviation TeamB { get; set; }

        public DateTime ScheduledDateTime { get; set; }
    }
}