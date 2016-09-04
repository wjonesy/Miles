using System.Data.Entity.Migrations;

namespace Miles.Sample.Persistence.EF.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SampleDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
