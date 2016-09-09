using Miles.Sample.Domain.Command.Leagues;
using Miles.Sample.Domain.Command.Teams;
using System;

namespace Miles.Sample.Domain.Command.Fixtures
{
    public class FixtureId
    {
        public static FixtureId Parse(string value)
        {
            FixtureId fixtureId;
            if (!TryParse(value, out fixtureId))
                throw new FormatException("The value is not a FixtureId format");

            return fixtureId;
        }

        public static bool TryParse(string value, out FixtureId fixtureId)
        {
            fixtureId = null;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            fixtureId = new Fixtures.FixtureId(value);
            return true;
        }

        public static FixtureId Generate(
            LeagueAbbreviation league,
            TeamAbbreviation teamA,
            TeamAbbreviation teamB,
            DateTime scheduledDateTime)
        {
            return new FixtureId(String.Format("{0}-{1}-{2}-{3:yyyyMMdd-Hmm}",
                league,
                teamA,
                teamB,
                scheduledDateTime));
        }

        protected FixtureId()
        { }

        private FixtureId(string id)
        {
            this.Id = id;
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
