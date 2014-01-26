namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClusterCoordinator : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClusterCoordinator",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PersonId = c.Int(nullable: false),
                        DisasterId = c.Int(nullable: false),
                        ClusterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Disaster", t => t.DisasterId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.Cluster", t => t.ClusterId, cascadeDelete: true)
                .Index(t => t.DisasterId)
                .Index(t => t.PersonId)
                .Index(t => t.ClusterId);
            
            CreateTable(
                "dbo.ClusterCoordinatorLogEntry",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeStampUtc = c.DateTime(nullable: false),
                        Event = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        PersonName = c.String(),
                        DisasterId = c.Int(nullable: false),
                        DisasterName = c.String(),
                        ClusterId = c.Int(nullable: false),
                        ClusterName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Cluster", "Name", c => c.String());
            AddForeignKey("dbo.Commitment", "PersonId", "dbo.Person", "Id", cascadeDelete: true);
            CreateIndex("dbo.Commitment", "PersonId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ClusterCoordinator", new[] { "ClusterId" });
            DropIndex("dbo.ClusterCoordinator", new[] { "PersonId" });
            DropIndex("dbo.ClusterCoordinator", new[] { "DisasterId" });
            DropIndex("dbo.Commitment", new[] { "PersonId" });
            DropForeignKey("dbo.ClusterCoordinator", "ClusterId", "dbo.Cluster");
            DropForeignKey("dbo.ClusterCoordinator", "PersonId", "dbo.Person");
            DropForeignKey("dbo.ClusterCoordinator", "DisasterId", "dbo.Disaster");
            DropForeignKey("dbo.Commitment", "PersonId", "dbo.Person");
            AlterColumn("dbo.Cluster", "Name", c => c.Int(nullable: false));
            DropTable("dbo.ClusterCoordinatorLogEntry");
            DropTable("dbo.ClusterCoordinator");
        }
    }
}
