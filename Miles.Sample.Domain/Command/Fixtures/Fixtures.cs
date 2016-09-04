using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System;

namespace Miles.Sample.Domain.Command.Fixtures
{
    public class Fixture
    {
        protected Fixture()
        { }

        public Fixture(FixtureId id, LeagueAbbreviation league, TeamAbbreviation teamA, TeamAbbreviation teamB, DateTime scheduledDateTime)
        {
            if (id == null)
                throw new ArgumentNullException("id");
            if (league == null)
                throw new ArgumentNullException("league");
            if (teamA == null)
                throw new ArgumentNullException("teamA");
            if (teamB == null)
                throw new ArgumentNullException("teamB");

            this.Id = id;
            this.League = league;
            this.TeamA = teamA;
            this.TeamB = teamB;
            State = FixtureStates.Pending;
            this.ScheduledDateTime = scheduledDateTime;
        }

        public FixtureId Id { get; private set; }

        public LeagueAbbreviation League { get; private set; }

        public TeamAbbreviation TeamA { get; private set; }

        public int TeamAPoints { get; private set; }

        public TeamAbbreviation TeamB { get; private set; }

        public int TeamBPoints { get; private set; }

        public FixtureStates State { get; private set; }

        public DateTime ScheduledDateTime { get; private set; }

        public DateTime? Started { get; set; }

        public DateTime? Finished { get; set; }

        public void Start(DomainContext domainContext)
        {
            if (State != FixtureStates.Pending)
                throw new InvalidOperationException("Fixture has already started or completed");

            State = FixtureStates.InProgress;
            Started = domainContext.Time.Now;

            domainContext.EventPublisher.Publish(new FixtureStarted(Started.Value));
        }

        public void Finish(DomainContext domainContext)
        {
            if (State != FixtureStates.InProgress)
                throw new InvalidOperationException("Fixture has not started or already completed");

            State = FixtureStates.Finished;
            Finished = domainContext.Time.Now;

            FixtureResults result;
            if (TeamAPoints > TeamBPoints)
                result = FixtureResults.TeamAWins;
            else if (TeamAPoints < TeamBPoints)
                result = FixtureResults.TeamBWins;
            else
                result = FixtureResults.Draw;

            domainContext.EventPublisher.Publish(new FixtureFinished(Finished.Value, this.Id, this.League, result, this.TeamA, TeamAPoints, this.TeamB, TeamBPoints));
        }
    }
}
