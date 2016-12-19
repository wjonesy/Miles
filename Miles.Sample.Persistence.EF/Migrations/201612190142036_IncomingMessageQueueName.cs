namespace Miles.Sample.Persistence.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncomingMessageQueueName : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.IncomingMessages");
            AddColumn("dbo.IncomingMessages", "QueueName", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.IncomingMessages", new[] { "MessageId", "QueueName" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.IncomingMessages");
            DropColumn("dbo.IncomingMessages", "QueueName");
            AddPrimaryKey("dbo.IncomingMessages", "MessageId");
        }
    }
}
