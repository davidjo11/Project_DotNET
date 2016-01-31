using FluentValidation;
using FluentValidation.Attributes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        public DateTime birthday { get; set; }

        public DateTime firstDay { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        //Matricule
        public string Pseudo { get; set; }

        public virtual ICollection<Period> Periods { get; set; }

        //Métier actuell 
        public string Job { get; set; }

        public bool addPeriod(Period Period)
        {
            var size = this.Periods.Count;
            this.Periods.Add(Period);
            if(size + 1 == this.Periods.Count)
            {
                //this.Periods.Sort((x, y) => DateTime.Compare(x.debut, y.debut));
                this.Periods.OrderBy(x => x.debut);
                //Actualisation de la période en cours (ou derniere période)
                //this.Period = this.Periods.Last();
                //Actualisation du Job courant
                //this.Job = this.Periods.Last().Job.JobName;
                //Actualisation firstDay
                this.firstDay = this.Periods.Last().debut;
                return true;
            }
            return false;
        }
        

        //Comme l'annotation de base key n'est pas recommandée j'ai utilisé FluentValidation: http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
        public class UserValidator : AbstractValidator<ApplicationUser>
        {
            public UserValidator()
            {
                RuleFor(x => x.firstName).NotEmpty().WithMessage("Le prénom est requis (2-100 lettres).").Length(2, 100);
                RuleFor(x => x.lastName).NotEmpty().WithMessage("Le nom est requis (2-100 lettres)..").Length(2, 100);
                //Age adulte requis
                RuleFor(x => DateTime.Now.Year - x.birthday.Year).GreaterThanOrEqualTo(18).WithMessage("Petit, tu dois avoir plus de 18 ans pour pouvoir travailler.");
                RuleFor(x => x).Must(x => { return correctDates(x.birthday, x.firstDay); }).WithMessage("La date d'entrée dans l'entreprise doit être strictement supérieure à la date de naissance.");
                RuleForEach(x => x.Periods).SetValidator(new Period.PeriodValidator());
            }
            
            private bool correctDates(DateTime birthday, DateTime firstDay)
            {
                return birthday.CompareTo(firstDay) == -1 ? true : false ;
            }
        }

        public string dateToString()
        {
            return this.birthday.ToString("dd/MM/RR");
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
            : base("NewCo", throwIfV1Schema: false)
        {}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Period> Periods { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Job> Jobs { get; set; }
    }
}