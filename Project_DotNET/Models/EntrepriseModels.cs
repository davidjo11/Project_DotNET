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
    public class Entreprise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Pour que la BDD génère auto. un ID unique.
        public int id { get; set; }

        [Required]
        public string name { get; set; }


        //Comme l'annotation de base key n'est pas recommandée j'ai utilisé FluentValidation: http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
        public class EntrepriseValidator : AbstractValidator<Entreprise>
        {
            public EntrepriseValidator()
            {
                RuleFor(x => x.name).NotEmpty().WithMessage("Le nom d'une entreprise est requis.").Length(0, 100);
                RuleFor(x => x.name).Must(BeUniqueUrl).WithMessage("Ce nom de cette entreprise existe déjà.");
            }

            private bool BeUniqueUrl(string name)
            {
                var _db = new EntrepriseDbContext();
                if (_db.Entreprises.SingleOrDefault(x => x.name == name) == null) return true;
                return false;
            }
        }

        public class EntrepriseDbContext : DbContext
        {
            public EntrepriseDbContext()
                : base("DefaultConnection")
            { }

            public static EntrepriseDbContext create()
            {
                return new EntrepriseDbContext();
            }

            public DbSet<Entreprise> Entreprises { get; set; }
        }
    }
}
