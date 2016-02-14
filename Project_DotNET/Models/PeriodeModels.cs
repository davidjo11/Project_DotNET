using FluentValidation;
using FluentValidation.Attributes;
using Project_DotNET.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Models
{
    public class Period
    {
        public bool En_Cours { get; set; }

        public int PeriodId { get; set; }

        [Required]
        [Column(TypeName = "DateTime2")]
        public DateTime debut { get; set; }

        //Nullable car passée ou en cours
        [Column(TypeName = "DateTime2")]
        public DateTime fin { get; set; }

        public int JobId { get; set; }

        public virtual Job Job { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }


        //Comme l'annotation de base key n'est pas recommandée j'ai utilisé FluentValidation: http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
        public class PeriodValidator : AbstractValidator<Period>
        {
            public PeriodValidator()
            {
                RuleFor(x => x.User).NotNull().WithMessage("Indiquer à quel utilisateur cette période doit être associée.");
                //La date de fin si elle n'est pas nulle doit être < à la date courante
                RuleFor(x => x.fin).LessThanOrEqualTo(DateTime.Now).When(x => x.fin != null).WithMessage("La date de fin ne peut être inférieure à aujourd'hui.");
                //La date de début ne peut être nulle 
                RuleFor(x => x.debut).NotNull().WithMessage("La date de début ne peut être nulle");
                //La date de début ne peut être supérieure ou égale à la date de fin (si non nulle) ou courante (aujourd'hui)
                //RuleFor(x => x.debut).LessThanOrEqualTo(DateTime.Now).When(x => x.fin == null).WithMessage("La date de début doit être antérieure à la date d'aujourd'hui.");
                RuleFor(x => x.debut).LessThanOrEqualTo(x => x.fin).When(x => x.fin != null).WithMessage("La date de début doit être inférieure à la date de fin si celle-ci n'est pas nulle");
                //La période se trouve avant ou après toutes les périodes existantes
                //RuleFor(x => x.debut).Must(noIntersection).WithMessage("L'utilisateur travaillait déjà dans une autre Company à cette date.");
                //RuleFor(x => new DateTime[]{ x.debut, x.fin}).Must(noIntersection).WithMessage("Une partie de cette période est déjà utilisée.");
                RuleFor(x => x).Must(x => { return noIntersection(x.User, x.debut, x.fin, x.En_Cours); }).WithMessage("La période se croise avec une autre période.");
            }

            private bool noIntersection(ApplicationUser user, DateTime debut, DateTime fin, bool en_cours)
            {
                var _db = new ApplicationDbContext();
                var Periods = 0;

                if (debut.CompareTo(user.firstDay) >= 0 || fin.CompareTo(user.firstDay) >= 0)
                    return false;

                //La période est correcte pour 2 cas:
                // - soit il n'existe aucune période pour laquelle la date de début est inférieure à la date de fin en param.
                // - soit il n'existe aucune période pour laquelle la date de fin est supérieure à la date de deb. en param.
                //Périodes qui croisent pas la nouvelle: http://stackoverflow.com/questions/13513932/algorithm-to-detect-overlapping-periods
                Periods = 0;
                /*Periods =
                    _db.Periods
                    .Select(x => x)
                    .Where(x => x.User.Id == user.Id && x.fin != null && (x.debut.CompareTo(fin) == 1 && x.fin.CompareTo(debut) == -1)).Count();
                if (p_en_cours != null)
                    Periods++;
                    */
                Periods =
                    _db.Periods
                    .Select(x => x)
                    .Where(x => x.User.Id == user.Id && (Tools.InclusiveBetween(debut, x.debut, x.fin) || Tools.InclusiveBetween(fin, x.debut, x.fin) || (Tools.InclusiveBetween(x.debut, debut, fin) || Tools.InclusiveBetween(x.fin, debut, fin)))).Count();

                //Si'il n'y a aucune intersection alors le nb de périodes est au nb de périodes en bases.
                return (Periods == 0) ? true : false;
            }
            
        }

        public bool isEnCours()
        {
            return this.En_Cours;
        }
    }
    /*public class PeriodDbContext : DbContext
    {
        public PeriodDbContext()
            : base("NewCo")
        { }

        public static PeriodDbContext create()
        {
            return new PeriodDbContext();
        }

        public DbSet<Period> Periods { get; set; }
    }*/
}
