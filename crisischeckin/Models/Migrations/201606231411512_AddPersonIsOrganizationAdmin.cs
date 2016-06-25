namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPersonIsOrganizationAdmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "IsOrganizationAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "IsOrganizationAdmin");
        }
    }
}
