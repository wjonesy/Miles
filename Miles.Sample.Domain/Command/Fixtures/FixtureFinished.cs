using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System;

namespace Miles.Sample.Domain.Command.Fixtures
{
    public class FixtureFinished
    {
        public FixtureFinished(DateTime when, FixtureId fixture, LeagueAbbreviation league, FixtureResults result, TeamAbbreviation teamA, int teamAPoints, TeamAbbreviation teamB, int teamBPoints)
        {
            this.When = when;
            this.Fixture = fixture.ToString();
            this.League = league.ToString();
            this.Result = (int)result;
            this.TeamA = new FixtureTeam(teamA.ToString(), teamAPoints);
            this.TeamB = new FixtureTeam(teamB.ToString(), teamBPoints);
        }

        public DateTime When { get; private set; }

        public string Fixture { get; private set; }

        public string League { get; private set; }

        public int Result { get; private set; }

        public FixtureTeam TeamA { get; private set; }

        public FixtureTeam TeamB { get; private set; }

        public class FixtureTeam
        {
            public FixtureTeam(string abbr, int points)
            {
                this.Abbreviation = abbr;
                this.Points = points;
            }

            public string Abbreviation { get; private set; }

            public int Points { get; private set; }
        }
    }
}