using Miles.Sample.Domain.Command.Teams;
using System;

namespace Miles.Sample.Domain.Command.Leagues
{
    class GoalRecorded
    {
        public LeagueAbbreviation League { get; set; }

        public Guid Id { get; set; }

        public DateTime When { get; set; }

        public TeamAbbreviation Team { get; set; }
    }
}