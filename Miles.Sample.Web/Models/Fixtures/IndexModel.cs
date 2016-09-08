using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Miles.Sample.Web.Models.Fixtures
{
    public class IndexModel
    {
        public string LeagueId { get; set; }

        public List<string> Teams { get; set; }

        public string TeamA { get; set; }

        public string TeamB { get; set; }

        public DateTime ScheduledDateTime { get; set; }

        public List<IndexModelFixture> Fixtures { get; set; }
    }

    public class IndexModelFixture
    {
        public string TeamA { get; set; }

        public int? TeamAPoints { get; set; }

        public string TeamB { get; set; }

        public int? TeamBPoints { get; set; }

        public DateTime ScheduledDateTime { get; set; }

        public bool Active { get; set; }

        public DateTime? Completed { get; set; }
    }
}