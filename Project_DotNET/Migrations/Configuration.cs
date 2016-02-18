namespace Project_DotNET.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Project_DotNET.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Project_DotNET.Models.ApplicationDbContext context)
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

            context.CustomRoles.AddOrUpdate(
                r => r.CustomRoleId,
                new Models.CustomRole { CustomRoleId = 1, RoleName = "Responsable fonctionnel", RoleDesc = "Tout ce qui est responsabilité fonctionnelle.", },
                new Models.CustomRole { CustomRoleId = 2, RoleName = "Responsable technique", RoleDesc = "Tout ce qui est responsabilité technique." },
                new Models.CustomRole { CustomRoleId = 3, RoleName = "Référent fonctionnel", RoleDesc = "Tout ce qui est...?" },
                new Models.CustomRole { CustomRoleId = 4, RoleName = "Référent technique", RoleDesc = "Tout ce qui est...?" }
                );

            context.SaveChanges();
        }
    }
}
