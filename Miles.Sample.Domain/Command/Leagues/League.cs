using Miles.Aggregates;
using Miles.Sample.Domain.Command.Teams;
using System;
using System.Linq;

namespace Miles.Sample.Domain.Command.Leagues
{
    public class League : Aggregate<LeagueState>
    {
        public League(LeagueAbbreviation abbreviation, string name)
        {
            if (abbreviation == null)
                throw new ArgumentNullException(nameof(abbreviation));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            this.ApplyNewEvent(new LeagueCreated
            {
                Id = abbreviation,
                Name = name
            });
        }

        public void RegisterTeam(TeamAbbreviation team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            if (State.State != LeagueState.LeagueStates.Planning)
                throw new InvalidOperationException("The league is not open for planning");

            if (State.RegisteredTeams.Contains(team))
                return;

            this.ApplyNewEvent(new TeamRegistered
            {
                Id = State.Id,
                Team = team
            });
        }

        public void StartLeague(DateTime when)
        {
            if (State.State == LeagueState.LeagueStates.InProgress)
                return;

            if (State.State == LeagueState.LeagueStates.Completed)
                throw new InvalidOperationException("League has already completed");

            this.ApplyNewEvent(new LeagueStarted
            {
                Id = State.Id,
                When = when
            });
        }

        public Guid ScheduleFixture(TeamAbbreviation teamA, TeamAbbreviation teamB, DateTime scheduledDateTime)
        {
            if (teamA == null)
                throw new ArgumentNullException(nameof(teamA));
            if (teamB == null)
                throw new ArgumentNullException(nameof(teamB));
            if (teamA == teamB)
                throw new ArgumentOutOfRangeException(nameof(teamB), "Team A and Team B cannot be the same team");

            if (!State.RegisteredTeams.Contains(teamA))
                throw new ArgumentOutOfRangeException(nameof(teamA), "Not registered with the league");
            if (!State.RegisteredTeams.Contains(teamB))
                throw new ArgumentOutOfRangeException(nameof(teamA), "Not registered with the league");

            var fixtureId = Guid.NewGuid();
            this.ApplyNewEvent(new FixtureScheduled
            {
                Id = fixtureId,
                League = State.Id,
                TeamA = teamA,
                TeamB = teamB,
                ScheduledDateTime = scheduledDateTime
            });
            return fixtureId;
        }

        public void StartFixture(Guid fixtureId, DateTime when)
        {
            if (State.HasFixtureStarted(fixtureId))
                throw new InvalidOperationException("Fixture has already started and possibly completed");

            this.ApplyNewEvent(new FixtureStarted
            {
                Id = fixtureId,
                League = State.Id,
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

            this.ApplyNewEvent(new GoalRecorded
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

        //public Fixture ScheduleFixture(TeamAbbreviation teamA, TeamAbbreviation teamB, DateTime scheduledDateTime)
        //{
        //    if (!State.RegisteredTeams.Contains(teamA))
        //        throw new ArgumentOutOfRangeException(nameof(teamA), "Not registered with the league");


        //    if (!State.RegisteredTeams.Contains(teamB))
        //        throw new ArgumentOutOfRangeException(nameof(teamB), "Not registered with the league");

        //    // TODO: Scheduling
        //    return new Fixture(Guid.NewGuid(), State.Id, teamA, teamB, scheduledDateTime);
        //}

        //public void RecordResult(FixtureResults result, TeamAbbreviation teamA, int teamAPoints, TeamAbbreviation teamB, int teamBPoints)
        //{
        //    var teamAStanding = Standings.SingleOrDefault(x => x.Team == teamA);
        //    if (teamAStanding == null)
        //        throw new ArgumentException("Team A is not a member of the league");

        //    var teamBStanding = Standings.SingleOrDefault(x => x.Team == teamB);
        //    if (teamBStanding == null)
        //        throw new ArgumentException("Team B is not a member of the league");

        //    switch (result)
        //    {
        //        case FixtureResults.TeamAWins:
        //            teamAStanding.RecordResult(LeagueStanding.Results.Win, teamAPoints, teamBPoints);
        //            teamBStanding.RecordResult(LeagueStanding.Results.Lose, teamBPoints, teamAPoints);
        //            break;
        //        case FixtureResults.TeamBWins:
        //            teamAStanding.RecordResult(LeagueStanding.Results.Lose, teamAPoints, teamBPoints);
        //            teamBStanding.RecordResult(LeagueStanding.Results.Win, teamBPoints, teamAPoints);
        //            break;
        //        case FixtureResults.Draw:
        //            teamAStanding.RecordResult(LeagueStanding.Results.Draw, teamAPoints, teamBPoints);
        //            teamBStanding.RecordResult(LeagueStanding.Results.Draw, teamBPoints, teamAPoints);
        //            break;
        //    }
        //}
    }
}
