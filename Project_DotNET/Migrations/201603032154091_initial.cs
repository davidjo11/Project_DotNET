namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AppRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AvailableRoles", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AvailableRoles", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AppRoles", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.Periods", "AppRoleId", c => c.Int(nullable: false));
            CreateIndex("dbo.Periods", "AppRoleId");
            AddForeignKey("dbo.Periods", "AppRoleId", "dbo.AppRoles", "AppRoleId", cascadeDelete: true);
            DropColumn("dbo.AvailableRoles", "ApplicationUser_Id");
            DropColumn("dbo.AppRoles", "AppRoleName");
            DropColumn("dbo.AppRoles", "AppRoleDesc");
            DropColumn("dbo.AppRoles", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppRoles", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.AppRoles", "AppRoleDesc", c => c.String());
            AddColumn("dbo.AppRoles", "AppRoleName", c => c.String());
            AddColumn("dbo.AvailableRoles", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Periods", "AppRoleId", "dbo.AppRoles");
            DropIndex("dbo.Periods", new[] { "AppRoleId" });
            DropColumn("dbo.Periods", "AppRoleId");
            CreateIndex("dbo.AppRoles", "ApplicationUser_Id");
            CreateIndex("dbo.AvailableRoles", "ApplicationUser_Id");
            AddForeignKey("dbo.AvailableRoles", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AppRoles", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
