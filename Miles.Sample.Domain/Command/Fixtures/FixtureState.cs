using Miles.Aggregates;
using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System;

namespace Miles.Sample.Domain.Command.Fixtures
{
    public class FixtureState : IAppliesEvent<FixtureScheduled>, IAppliesEvent<FixtureStarted>, IAppliesEvent<GoalScored>, IAppliesEvent<FixtureFinished>
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

        void IAppliesEvent<FixtureScheduled>.ApplyEvent(FixtureScheduled @event)
        {
            this.Id = @event.Id;
            this.League = @event.League;
            this.TeamA = @event.TeamA;
            this.TeamB = @event.TeamB;
            this.ScheduledDateTime = @event.ScheduledDateTime;
        }

        void IAppliesEvent<FixtureStarted>.ApplyEvent(FixtureStarted @event)
        {
            this.Started = @event.When;
            this.State = FixtureStates.InProgress;
        }

        void IAppliesEvent<GoalScored>.ApplyEvent(GoalScored @event)
        {
            if (@event.Team == TeamA)
                ++TeamAPoints;
            else
                ++TeamBPoints;
        }

        void IAppliesEvent<FixtureFinished>.ApplyEvent(FixtureFinished @event)
        {
            this.Finished = @event.When;
            this.State = FixtureStates.Finished;
        }
    }
}
