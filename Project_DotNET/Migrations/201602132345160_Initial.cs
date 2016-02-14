namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                        CategoryDesc = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        JobId = c.Int(nullable: false, identity: true),
                        JobName = c.String(nullable: false),
                        JobDesc = c.String(),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(nullable: false),
                        country = c.String(nullable: false),
                        city = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Periods",
                c => new
                    {
                        PeriodId = c.Int(nullable: false, identity: true),
                        En_Cours = c.Boolean(nullable: false),
                        debut = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        fin = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        JobId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PeriodId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.JobId)
                .Index(t => t.CompanyId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        birthday = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        firstDay = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CompanyId = c.Int(nullable: false),
                        JobId = c.Int(nullable: false),
                        firstName = c.String(nullable: false),
                        lastName = c.String(nullable: false),
                        Pseudo = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Jobs", t => t.JobId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.JobId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Periods", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.AspNetUsers", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Periods", "JobId", "dbo.Jobs");
            DropForeignKey("dbo.Periods", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Jobs", "CategoryId", "dbo.Categories");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "JobId" });
            DropIndex("dbo.AspNetUsers", new[] { "CompanyId" });
            DropIndex("dbo.Periods", new[] { "UserId" });
            DropIndex("dbo.Periods", new[] { "CompanyId" });
            DropIndex("dbo.Periods", new[] { "JobId" });
            DropIndex("dbo.Jobs", new[] { "CategoryId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Periods");
            DropTable("dbo.Companies");
            DropTable("dbo.Jobs");
            DropTable("dbo.Categories");
        }
    }
}
