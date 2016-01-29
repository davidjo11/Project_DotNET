namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pseudo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Pseudo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Pseudo");
        }
    }
}
