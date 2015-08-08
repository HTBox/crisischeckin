namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ODFThatConf2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Person", "FK_dbo.Person_dbo.Cluster_ClusterId");
            DropForeignKey("dbo.Person", "Cluster_Id", "dbo.Cluster");
            DropIndex("dbo.Person", new[] { "Cluster_Id" });
            CreateTable(
                "dbo.DisasterCluster",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClusterId = c.Int(nullable: false),
                        DisasterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cluster", t => t.ClusterId, cascadeDelete: true)
                .ForeignKey("dbo.Disaster", t => t.DisasterId, cascadeDelete: true)
                .Index(t => t.ClusterId)
                .Index(t => t.DisasterId);

            CreateIndex("dbo.Commitment", "ClusterId");
            AddForeignKey("dbo.Commitment", "ClusterId", "dbo.Cluster", "Id");
            DropColumn("dbo.Person", "Cluster_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.Person", "Cluster_Id", c => c.Int());
            DropForeignKey("dbo.DisasterCluster", "DisasterId", "dbo.Disaster");
            DropForeignKey("dbo.DisasterCluster", "ClusterId", "dbo.Cluster");
            DropForeignKey("dbo.Commitment", "ClusterId", "dbo.Cluster");
            DropIndex("dbo.DisasterCluster", new[] { "DisasterId" });
            DropIndex("dbo.DisasterCluster", new[] { "ClusterId" });
            DropIndex("dbo.Commitment", new[] { "ClusterId" });
            DropTable("dbo.DisasterCluster");
            CreateIndex("dbo.Person", "Cluster_Id");
            AddForeignKey("dbo.Person", "Cluster_Id", "dbo.Cluster", "Id");
        }
    }
}