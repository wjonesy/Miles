namespace Miles.Sample.WebReadStore.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Miles.Sample.WebReadStore.EF.WebReadStoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Miles.Sample.WebReadStore.EF.WebReadStoreContext context)
        { }
    }
}
