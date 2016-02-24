
using Microsoft.AspNet.Identity.EntityFramework;
using Project_DotNET.Models;
using System.Data.Entity;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext()
        : base("NewCo", throwIfV1Schema: false)
    {
        //Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseAlways<ApplicationDbContext>()); //Drop database every times
        //Database.Initialize(true);
    }

    public static ApplicationDbContext Create()
    {
        return new ApplicationDbContext();
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Period> Periods { get; set; }

    public DbSet<Company> Companies { get; set; }

    public DbSet<Job> Jobs { get; set; }

    public DbSet<AvailableRole> AvailableRoles { get; set; }
}