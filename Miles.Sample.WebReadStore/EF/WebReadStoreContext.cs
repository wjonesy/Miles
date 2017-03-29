using Miles.Sample.WebReadStore.Schema;
using System.Data.Entity;

namespace Miles.Sample.WebReadStore.EF
{
    public class WebReadStoreContext : DbContext
    {
        public WebReadStoreContext() : base("Miles.Sample.WebReadStore")
        { }

        public DbSet<League> Leagues { get; set; }
        public DbSet<LeagueStanding> LeagueStandings { get; set; }
    }
}
