namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using Project_DotNET.Models;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Categories.AddOrUpdate(

                c => c.CategoryId,
                new Models.Category { CategoryId = 1, CategoryName = "Achats" },
                new Models.Category { CategoryId = 2, CategoryName = "IT" });

            context.Companies.AddOrUpdate(
                c => c.CompanyId,
                new Models.Company { CompanyId = 1, CompanyName = "Capgemini", city = "Lambersart", country = "France" },
                new Models.Company { CompanyId = 2, CompanyName = "Atos", city = "Lille", country = "France" },
                new Models.Company { CompanyId = 3, CompanyName = "Capgemini", city = "Paris", country = "France" },
                new Models.Company { CompanyId = 4, CompanyName = "QuaddraDiffusion", city = "Villeneuve-d'Ascq", country = "France" },
                new Models.Company { CompanyId = 5, CompanyName = "Unis", city = "Villeneuve-D'Ascq", country = "France" },
                new Models.Company { CompanyId = 6, CompanyName = "GFI", city = "Lille", country = "France" },
                new Models.Company { CompanyId = 7, CompanyName = "CGI", city = "Lille", country = "France" }
                );

            context.Jobs.AddOrUpdate(
                j => j.JobId,
                new Models.Job { JobId = 1, CategoryId = 2, JobName = "Développeur" },
                new Models.Job { JobId = 2, CategoryId = 1, JobName = "Acheteur" }
                );

            context.AvailableRoles.AddOrUpdate(
                r => r.AvailableRoleId,
                new Models.AvailableRole { AvailableRoleId = 1, AvailableRoleName = "Manager" ,AvailableRoleDesc="ceci est un manager"},
                new Models.AvailableRole { AvailableRoleId = 2, AvailableRoleName = "Chef d'équipe", AvailableRoleDesc = "ceci est un chef d'équipe " },
                new Models.AvailableRole { AvailableRoleId = 3, AvailableRoleName = "Responsable fonctionnel", AvailableRoleDesc = "ceci est un resp fonc" },
                new Models.AvailableRole { AvailableRoleId = 4, AvailableRoleName = "Responsable technique", AvailableRoleDesc = "ceci est un resp tec" },
                new Models.AvailableRole { AvailableRoleId = 5, AvailableRoleName = "Apprenti", AvailableRoleDesc = "ceci est un apprenti" },
                new Models.AvailableRole { AvailableRoleId = 6, AvailableRoleName = "Coordinateur", AvailableRoleDesc = "ceci est un coordinateur" },
                new Models.AvailableRole { AvailableRoleId = 7, AvailableRoleName = "Référent fonctionnel", AvailableRoleDesc = "ceci est un ref fonc" },
                new Models.AvailableRole { AvailableRoleId = 8, AvailableRoleName = "Référent technique", AvailableRoleDesc = "ceci est un ref tec" }
                );
                
                context.SaveChanges();
        }
    }
}
