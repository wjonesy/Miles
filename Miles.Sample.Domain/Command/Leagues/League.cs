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

            if (State.RegisteredTeams.Contains(team))   // Team already registered, ignore
                return;

            this.ApplyNewEvent(new TeamRegistered
            {
                Id = State.Id,
                Team = team
            });
        }

        public void Start(DateTime when)
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
