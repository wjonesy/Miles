namespace Miles.Sample.WebReadStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Leagues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeagueId = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeagueStandings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeamId = c.String(),
                        TeamName = c.String(),
                        GamesPlayed = c.Int(nullable: false),
                        GamesWon = c.Int(nullable: false),
                        GamesDrawn = c.Int(nullable: false),
                        GamesLost = c.Int(nullable: false),
                        GoalsFor = c.Int(nullable: false),
                        GoalsAgainst = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        League_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leagues", t => t.League_Id)
                .Index(t => t.League_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeagueStandings", "League_Id", "dbo.Leagues");
            DropIndex("dbo.LeagueStandings", new[] { "League_Id" });
            DropTable("dbo.LeagueStandings");
            DropTable("dbo.Leagues");
        }
    }
}
