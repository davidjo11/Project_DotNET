README.MD

Notes:

Download sqlloaderdb.msi for local databases management, here https://www.microsoft.com/fr-FR/download/details.aspx?id=29062 .

Remember to go in the control management package in the Project (I think), to update the database, entering Update-Migration.


In progress...

About Entities Relationship checkout this website: 
http://www.entityframeworktutorial.net/code-first/configure-one-to-one-relationship-in-code-first.aspx .

About handling migration (update on models):
http://blogs.msdn.com/b/webdev/archive/2013/10/16/customizing-profile-information-in-asp-net-identity-in-vs-2013-templates.aspx
To better understand how to handle foreign key see these model architecture: 
https://github.com/rustd/AspnetIdentitySample/blob/master/AspnetIdentitySample/Models/AppModel.cs .
This one didn't really helped but gonna try to make it work later: http://stackoverflow.com/questions/22297097/how-to-add-a-foreign-key-reference-to-asp-net-mvc-5-identity .
Do not forget that when you've got more than one primary key in your class your have to order them (https://msdn.microsoft.com/en-us/data/jj591583#Composite).

About the email service configuration:

- http://bitoftech.net/2015/02/03/asp-net-identity-2-accounts-confirmation-password-user-policy-configuration/ : using the included user manager that the link you want to check out for sure.

- to get more info about the configuration ou other emailing services checkout these links: 
	- http://www.mikesdotnetting.com/article/268/how-to-send-email-in-asp-net-mvc
	- http://tech.trailmax.info/2014/09/sending-emails-in-asp-net-identity-using-dependency-injection-sendgrid-and-debugging-it-with-mailtrap-io/
	namespace Project_DotNET.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DropCreateDatabaseAlways<Project_DotNET.Models.ApplicationDbContext>
    {
        /*public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }*/

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
