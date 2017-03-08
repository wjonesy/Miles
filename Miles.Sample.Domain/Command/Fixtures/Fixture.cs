using Miles.Aggregates;
using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System;

namespace Miles.Sample.Domain.Command.Fixtures
{
    public class Fixture : Aggregate<FixtureState>
    {
        public Fixture()
        { }

        public Fixture(Guid id, LeagueAbbreviation league, TeamAbbreviation teamA, TeamAbbreviation teamB, DateTime scheduledDateTime)
        {
            if (league == null)
                throw new ArgumentNullException(nameof(league));
            if (teamA == null)
                throw new ArgumentNullException(nameof(teamA));
            if (teamB == null)
                throw new ArgumentNullException(nameof(teamB));
            if (teamA == teamB)
                throw new ArgumentOutOfRangeException(nameof(teamB), "Team A and Team B cannot be the same team");


            this.ApplyNewEvent(new FixtureScheduled
            {
                Id = id,
                League = league,
                TeamA = teamA,
                TeamB = teamB,
                ScheduledDateTime = scheduledDateTime
            });
        }

        public void Start(DateTime when)
        {
            if (State.State != FixtureState.FixtureStates.Scheduled)
                throw new InvalidOperationException("Fixture has already started or completed");

            this.ApplyNewEvent(new FixtureStarted
            {
                Id = this.State.Id,
                When = when
            });
        }

        public void RecordGoal(TeamAbbreviation team, DateTime when)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            if (team != State.TeamA && team != State.TeamB)
                throw new ArgumentOutOfRangeException(nameof(team), "Must be one of the team fixtures");

            if (State.State != FixtureState.FixtureStates.InProgress)
                throw new InvalidOperationException("Cannot score a goal while the game is not in-progress");

            if (when < State.Started.Value)
                throw new ArgumentOutOfRangeException(nameof(when), "Cannot score a goal before the game has started.");

            this.ApplyNewEvent(new GoalScored
            {
                Id = State.Id,
                When = when,
                Team = team
            });
        }

        public void Finish(DateTime when)
        {
            if (State.State != FixtureState.FixtureStates.InProgress)
                throw new InvalidOperationException("Fixture has not started or already completed");

            if (when <= State.Started.Value)
                throw new ArgumentOutOfRangeException(nameof(when), "Cannot finish the fixture before it started.");

            FixtureResults result;
            if (State.TeamAPoints > State.TeamBPoints)
                result = FixtureResults.TeamAWins;
            else if (State.TeamAPoints < State.TeamBPoints)
                result = FixtureResults.TeamBWins;
            else
                result = FixtureResults.Draw;

            this.ApplyNewEvent(new FixtureFinished
            {
                Id = this.State.Id,
                When = when,
                Result = result
            });
        }
    }
}
