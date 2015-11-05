namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVolunteerType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VolunteerType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            // Since the previous version, we've added the 
            // Volunteer type table:
            Sql(String.Format("INSERT INTO VolunteerType (Name) VALUES ('{0}')", VolunteerType.VOLUNTEERTYPE_ONSITE));
            Sql(String.Format("INSERT INTO VolunteerType (Name) VALUES ('{0}')", VolunteerType.VOLUNTEERTYPE_REMOTE));
                        
            AddColumn("dbo.Commitment", "VolunteerTypeId", c => c.Int(nullable: false, defaultValue: 1));
            CreateIndex("dbo.Commitment", "VolunteerTypeId");
            AddForeignKey("dbo.Commitment", "VolunteerTypeId", "dbo.VolunteerType", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Commitment", "VolunteerTypeId", "dbo.VolunteerType");
            DropIndex("dbo.Commitment", new[] { "VolunteerTypeId" });
            DropColumn("dbo.Commitment", "VolunteerTypeId");
            DropTable("dbo.VolunteerType");
        }
    }
}
