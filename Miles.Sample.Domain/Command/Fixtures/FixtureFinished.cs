using System;

namespace Miles.Sample.Domain.Command.Fixtures
{
    public class FixtureFinished
    {
        public Guid Id { get; set; }

        public DateTime When { get; set; }

        public FixtureResults Result { get; set; }
    }
}