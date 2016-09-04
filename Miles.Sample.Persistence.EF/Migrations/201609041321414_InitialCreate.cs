namespace Miles.Sample.Persistence.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Fixtures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Id = c.String(nullable: false, maxLength: 50),
                        League_Abbreviation = c.String(nullable: false, maxLength: 6),
                        TeamA_Abbreviation = c.String(nullable: false, maxLength: 6),
                        TeamAPoints = c.Int(nullable: false),
                        TeamB_Abbreviation = c.String(nullable: false, maxLength: 6),
                        TeamBPoints = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        ScheduledDateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Started = c.DateTime(precision: 7, storeType: "datetime2"),
                        Finished = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncomingMessages",
                c => new
                    {
                        MessageId = c.Guid(nullable: false),
                        When = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.MessageId);
            
            CreateTable(
                "dbo.Leagues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abbreviation_Abbreviation = c.String(nullable: false, maxLength: 6),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeagueStandings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Team_Abbreviation = c.String(nullable: false, maxLength: 6),
                        Played = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        PointsFor = c.Int(nullable: false),
                        PointsAgainst = c.Int(nullable: false),
                        League_SurrogateId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Leagues", t => t.League_SurrogateId, cascadeDelete: true)
                .Index(t => t.League_SurrogateId);
            
            CreateTable(
                "dbo.OutgoingMessages",
                c => new
                    {
                        MessageId = c.Guid(nullable: false),
                        CorrelationId = c.Guid(nullable: false),
                        ClassTypeName = c.String(nullable: false, maxLength: 255),
                        ConceptType = c.Int(nullable: false),
                        SerializedMessage = c.String(),
                        EventCreated = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EventDispatched = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.MessageId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Abbreviation_Abbreviation = c.String(nullable: false, maxLength: 6),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LeagueStandings", "League_SurrogateId", "dbo.Leagues");
            DropIndex("dbo.LeagueStandings", new[] { "League_SurrogateId" });
            DropTable("dbo.Teams");
            DropTable("dbo.OutgoingMessages");
            DropTable("dbo.LeagueStandings");
            DropTable("dbo.Leagues");
            DropTable("dbo.IncomingMessages");
            DropTable("dbo.Fixtures");
        }
    }
}
