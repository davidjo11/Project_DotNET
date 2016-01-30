using FluentValidation;
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
    public class Periode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Pour que la BDD génère auto. un ID unique.
        public int id { get; set; }

        [Required]
        public DataType debut { get; set; }

        //Nullable car passée ou en cours
        public DataType fin { get; set; }

        [ForeignKey("id")]
        [Required]
        public Metier metier { get; set; }

        [ForeignKey("id")]
        [Required]
        public Entreprise entreprise { get; set; }


        //Comme l'annotation de base key n'est pas recommandée j'ai utilisé FluentValidation: http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
        public class PeriodeValidator : AbstractValidator<Periode>
        {
            /*public PeriodeValidator()
            {
                RuleFor(x => x.name).NotEmpty().WithMessage("Le nom d'une categorie est requis.").Length(0, 100);
                RuleFor(x => x.name).Must(BeUniqueUrl).WithMessage("Ce nom de categorie existe déjà.");
            }

            private bool BeUniqueUrl(string name)
            {
                var _db = new PeriodeDbContext();
                if (_db.Periodes.SingleOrDefault(x => x.name == name) == null) return true;
                return false;
            }*/
        }

        public class PeriodeDbContext : DbContext
        {
            public PeriodeDbContext()
                : base("DefaultConnection")
            { }

            public static PeriodeDbContext create()
            {
                return new PeriodeDbContext();
            }

            public DbSet<Periode> Periodes { get; set; }
        }
    }
}
