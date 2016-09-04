using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System;

namespace Miles.Sample.Domain.Command.Fixtures
{
    public class FixtureId
    {
        protected FixtureId()
        { }

        public FixtureId(
            LeagueAbbreviation league,
            TeamAbbreviation teamA,
            TeamAbbreviation teamB,
            DateTime scheduledDateTime)
        {
            Id = String.Format("{0}-{1}-{2}-{3:yyyyMMdd-Hmm}",
                league,
                teamA,
                teamB,
                scheduledDateTime);
        }

        public string Id { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var otherObj = obj as FixtureId;
            if (otherObj == null)
                return false;

            return this.Id.Equals(otherObj.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return this.Id;
        }
    }
}
