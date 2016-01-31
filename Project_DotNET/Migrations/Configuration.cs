namespace Project_DotNET.Migrations
{
    using System;
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

                c => c.Id,
                new Models.Category { Id = 1, CategoryName = "Achats" },
                new Models.Category { Id = 2, CategoryName = "IT" });

            context.Companies.AddOrUpdate(
                c => c.Id,
                new Models.Company { Id = 1, CompanyName = "Capgemini", city = "Lambersart", country = "France" },
                new Models.Company { Id = 2, CompanyName = "Atos", city = "Lille", country = "France" },
                new Models.Company { Id = 3, CompanyName = "Capgemini", city = "Paris", country = "France" },
                new Models.Company { Id = 4, CompanyName = "QuaddraDiffusion", city = "Villeneuve-d'Ascq", country = "France" },
                new Models.Company { Id = 5, CompanyName = "Unis", city = "Villeneuve-D'Ascq", country = "France" },
                new Models.Company { Id = 6, CompanyName = "GFI", city = "Lille", country = "France" },
                new Models.Company { Id = 7, CompanyName = "CGI", city = "Lille", country = "France" }
                );

            context.Jobs.AddOrUpdate(
                j => j.Id,
                new Models.Job { Id = 1, CategoryId = 2, JobName = "Développeur" },
                new Models.Job { Id = 2, CategoryId = 1, JobName = "Acheteur" }
                );

            context.SaveChanges();
        }
    }
}
