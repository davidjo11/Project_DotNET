namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rolescollection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "AvailableRoleId", "dbo.AvailableRoles");
            DropIndex("dbo.AspNetUsers", new[] { "AvailableRoleId" });
            AddColumn("dbo.AvailableRoles", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AvailableRoles", "ApplicationUser_Id");
            AddForeignKey("dbo.AvailableRoles", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.AspNetUsers", "AvailableRoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "AvailableRoleId", c => c.Int(nullable: false));
            DropForeignKey("dbo.AvailableRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AvailableRoles", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AvailableRoles", "ApplicationUser_Id");
            CreateIndex("dbo.AspNetUsers", "AvailableRoleId");
            AddForeignKey("dbo.AspNetUsers", "AvailableRoleId", "dbo.AvailableRoles", "AvailableRoleId", cascadeDelete: true);
        }
    }
}
