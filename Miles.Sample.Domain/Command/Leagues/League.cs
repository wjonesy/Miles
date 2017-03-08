using Miles.Sample.Domain.Command.Fixtures;
using Miles.Sample.Domain.Command.Teams;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Miles.Sample.Domain.Command.Leagues
{
    public class League
    {
        protected League()
        {
            Standings = new List<LeagueStanding>();
        }

        public League(LeagueAbbreviation abbreviation, string name) : this()
        {
            this.Abbreviation = abbreviation;
            this.Name = name;
        }

        public int SurrogateId { get; private set; }

        public LeagueAbbreviation Abbreviation { get; private set; }

        public string Name { get; private set; }

        public virtual ICollection<LeagueStanding> Standings { get; private set; }

        public void RegisterTeam(TeamAbbreviation team)
        {
            Standings.Add(new LeagueStanding(this, team));
        }

        public void RecordResult(FixtureResults result, TeamAbbreviation teamA, int teamAPoints, TeamAbbreviation teamB, int teamBPoints)
        {
            var teamAStanding = Standings.SingleOrDefault(x => x.Team == teamA);
            if (teamAStanding == null)
                throw new ArgumentException("Team A is not a member of the league");

            var teamBStanding = Standings.SingleOrDefault(x => x.Team == teamB);
            if (teamBStanding == null)
                throw new ArgumentException("Team B is not a member of the league");

            switch (result)
            {
                case FixtureResults.TeamAWins:
                    teamAStanding.RecordResult(LeagueStanding.Results.Win, teamAPoints, teamBPoints);
                    teamBStanding.RecordResult(LeagueStanding.Results.Lose, teamBPoints, teamAPoints);
                    break;
                case FixtureResults.TeamBWins:
                    teamAStanding.RecordResult(LeagueStanding.Results.Lose, teamAPoints, teamBPoints);
                    teamBStanding.RecordResult(LeagueStanding.Results.Win, teamBPoints, teamAPoints);
                    break;
                case FixtureResults.Draw:
                    teamAStanding.RecordResult(LeagueStanding.Results.Draw, teamAPoints, teamBPoints);
                    teamBStanding.RecordResult(LeagueStanding.Results.Draw, teamBPoints, teamAPoints);
                    break;
            }
        }

        public Fixture ScheduleFixture(DomainContext domainContext, TeamAbbreviation teamA, TeamAbbreviation teamB, DateTime scheduledDateTime)
        {
            var fixture = new Fixture(Abbreviation, teamA, teamB, scheduledDateTime);
            return fixture;
        }
    }
}
