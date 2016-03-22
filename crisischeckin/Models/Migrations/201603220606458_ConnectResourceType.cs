namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectResourceType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Resource", "Disaster_Id", "dbo.Disaster");
            DropIndex("dbo.Resource", new[] { "Disaster_Id" });
            RenameColumn(table: "dbo.Resource", name: "Disaster_Id", newName: "DisasterId");
            AddColumn("dbo.Resource", "ResourceTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Resource", "DisasterId", c => c.Int(nullable: false));
            CreateIndex("dbo.Resource", "DisasterId");
            CreateIndex("dbo.Resource", "ResourceTypeId");
            AddForeignKey("dbo.Resource", "ResourceTypeId", "dbo.ResourceType", "ResourceTypeId", cascadeDelete: true);
            AddForeignKey("dbo.Resource", "DisasterId", "dbo.Disaster", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resource", "DisasterId", "dbo.Disaster");
            DropForeignKey("dbo.Resource", "ResourceTypeId", "dbo.ResourceType");
            DropIndex("dbo.Resource", new[] { "ResourceTypeId" });
            DropIndex("dbo.Resource", new[] { "DisasterId" });
            AlterColumn("dbo.Resource", "DisasterId", c => c.Int());
            DropColumn("dbo.Resource", "ResourceTypeId");
            RenameColumn(table: "dbo.Resource", name: "DisasterId", newName: "Disaster_Id");
            CreateIndex("dbo.Resource", "Disaster_Id");
            AddForeignKey("dbo.Resource", "Disaster_Id", "dbo.Disaster", "Id");
        }
    }
}
