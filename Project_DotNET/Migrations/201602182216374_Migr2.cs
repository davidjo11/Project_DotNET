namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migr2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomRoles",
                c => new
                    {
                        CustomRoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        RolesIncId = c.Int(nullable: false),
                        RoleDesc = c.String(),
                    })
                .PrimaryKey(t => t.CustomRoleId)
                .ForeignKey("dbo.RolesIncs", t => t.RolesIncId, cascadeDelete: true)
                .Index(t => t.RolesIncId);
            
            CreateTable(
                "dbo.RolesIncs",
                c => new
                    {
                        RolesIncId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.RolesIncId);
            
            AddColumn("dbo.AspNetUsers", "RoleId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Role_CustomRoleId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Role_CustomRoleId");
            AddForeignKey("dbo.AspNetUsers", "Role_CustomRoleId", "dbo.CustomRoles", "CustomRoleId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Role_CustomRoleId", "dbo.CustomRoles");
            DropForeignKey("dbo.CustomRoles", "RolesIncId", "dbo.RolesIncs");
            DropIndex("dbo.AspNetUsers", new[] { "Role_CustomRoleId" });
            DropIndex("dbo.CustomRoles", new[] { "RolesIncId" });
            DropColumn("dbo.AspNetUsers", "Role_CustomRoleId");
            DropColumn("dbo.AspNetUsers", "RoleId");
            DropTable("dbo.RolesIncs");
            DropTable("dbo.CustomRoles");
        }
    }
}
