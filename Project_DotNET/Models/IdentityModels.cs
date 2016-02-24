using FluentValidation;
using FluentValidation.Attributes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project_DotNET.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project_DotNET.Models
{
    // Vous pouvez ajouter des données de profil pour l'utilisateur en ajoutant plus de propriétés à votre classe ApplicationUser ; consultez http://go.microsoft.com/fwlink/?LinkID=317594 pour en savoir davantage.
    [Validator(typeof(UserValidator))]
    public class    ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Periods = new List <Period>();
        }

        [Column(TypeName = "DateTime2")]
        public DateTime birthday { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime firstDay { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public int JobId { get; set; }

        public virtual Job Job { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        //Matricule
        public string Pseudo { get; set; }

        //public int PeriodId { get; set; }

        //public virtual Period LastPeriod { get; set; }

        public virtual ICollection<Period> Periods { get; set; }

        public bool addPeriod(Period Period)
        {
            var size = this.Periods.Count;
            this.Periods.Add(Period);
            this.Periods.OrderBy(x => x.debut);
            return size + 1 == Periods.Count;
        }

        public bool removeLastPeriod()
        {
            if (this.Periods.Count == 0)
                return false;

            var _db = new ApplicationDbContext().Periods;
            var size = this.Periods.Count;
            var removed = this.Periods.Remove(this.Periods.Last());
            this.Periods.OrderBy(x => x.debut);

            return this.Periods.Count == size -1;
        }

        public bool removePeriod(Period period)
        {
            if (this.Periods.Count == 0)
                return false;

            var _db = new ApplicationDbContext().Periods;
            var size = this.Periods.Count;
            this.Periods.OrderBy(x => x.debut);
            var removed = this.Periods.Remove(period);

            return this.Periods.Count == size - 1;
        }


        //Comme l'annotation de base key n'est pas recommandée j'ai utilisé FluentValidation: http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
        public class UserValidator : AbstractValidator<ApplicationUser>
        {
            public UserValidator()
            {
                RuleFor(x => x.firstName).NotEmpty().WithMessage("Le prénom est requis (2-10 lettres).").Length(2, 10);
                RuleFor(x => x.lastName).NotEmpty().WithMessage("Le nom est requis (2-10 lettres).").Length(2, 10);
                //Age adulte requis
                RuleFor(x => DateTime.Now.Year - x.birthday.Year).GreaterThanOrEqualTo(18).WithMessage("Petit, tu dois avoir plus de 18 ans pour pouvoir travailler.");
                RuleFor(x => x).Must(x => { return correctDates(x.birthday, x.firstDay); }).WithMessage("La date d'entrée dans l'entreprise doit être strictement supérieure à la date de naissance.");
                //RuleForEach(x => x.Periods).SetValidator(new Period.PeriodValidator());
            }
            
            private bool correctDates(DateTime birthday, DateTime firstDay)
            {
                return birthday.CompareTo(firstDay) == -1 ? true : false ;
            }
        }

        public string dateToString()
        {
            return this.birthday.ToString("dd-MM-yyyy");
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Notez qu'authenticationType doit correspondre à l'élément défini dans CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ajouter les revendications personnalisées de l’utilisateur ici
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base(Tools.connections[0], throwIfV1Schema: false)
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

        public DbSet<AppRole> AppRoles { get; set; }

        public DbSet<AvailableRole> AvailableRoles { get; set; }
    }
}