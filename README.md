README.MD

Things to do when you clone or pull the repo:

Create local database:
- Double-click on properties, in "Package/Publication SQL" activate the page, create a connection called NewCo and check the box right under "informations about the source database" (Infos sur la bdd source).
- go in the NuGet console and do Upddate-Database then enter (that's it the database should be set).

Package to install beforehand, do in console package NuGet:
- Install-Package FluentValidation.MVC5 -Version 6.1.0 
- Install-Package jQuery.Validation



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

Very interesting websites about everything:
- This one was helpful for the database initialization configuration: http://patrickdesjardins.com/blog/entity-framework-database-initialization
- About the email settings: you need http://stackoverflow.com/questions/29774401/how-to-send-email-from-the-c-sharp-program
- Issues with DropDownList in view: http://odetocode.com/Blogs/scott/archive/2010/01/18/drop-down-lists-and-asp-net-mvc.aspx
