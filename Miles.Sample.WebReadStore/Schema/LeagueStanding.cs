namespace Miles.Sample.WebReadStore.Schema
{
    public class LeagueStanding
    {
        public int Id { get; set; }

        public virtual League League { get; set; }

        public string TeamId { get; set; }

        public string TeamName { get; set; }

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }

        public int GamesDrawn { get; set; }

        public int GamesLost { get; set; }

        public int GoalsFor { get; set; }

        public int GoalsAgainst { get; set; }

        public int Points { get; set; }
    }
}
