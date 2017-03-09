using Miles.Sample.Domain.Command.Teams;
using System;

namespace Miles.Sample.Domain.Command.Leagues
{
    class GoalRecorded
    {
        public Guid Id { get; set; }

        public DateTime When { get; set; }

        public TeamAbbreviation Team { get; set; }
    }
}