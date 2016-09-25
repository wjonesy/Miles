namespace Miles.Sample.Persistence.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageRecordingChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OutgoingMessages", "CreatedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.OutgoingMessages", "DispatchedDate", c => c.DateTime(precision: 7, storeType: "datetime2"));
            DropColumn("dbo.OutgoingMessages", "EventCreated");
            DropColumn("dbo.OutgoingMessages", "EventDispatched");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OutgoingMessages", "EventDispatched", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AddColumn("dbo.OutgoingMessages", "EventCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("dbo.OutgoingMessages", "DispatchedDate");
            DropColumn("dbo.OutgoingMessages", "CreatedDate");
        }
    }
}
