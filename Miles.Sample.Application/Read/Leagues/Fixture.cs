using System;

namespace Miles.Sample.Application.Read.Leagues
{
    public class Fixture
    {
        public string Id { get; set; }

        public string TeamA { get; set; }

        public int? TeamAPoints { get; set; }

        public string TeamB { get; set; }

        public int? TeamBPoints { get; set; }

        public DateTime ScheduledDateTime { get; set; }

        public bool Active { get; set; }

        public DateTime? Completed { get; set; }
    }
}