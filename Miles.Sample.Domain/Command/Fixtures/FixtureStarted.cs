using System;

namespace Miles.Sample.Domain.Command.Fixtures
{
    class FixtureStarted
    {
        public FixtureStarted(DateTime when)
        {
            this.When = when;
        }

        public DateTime When { get; private set; }
    }
}