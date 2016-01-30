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
    public class Metier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Pour que la BDD génère auto. un ID unique.
        public string id { get; }
        [Required]
        public string name { get; set; }
        [ForeignKey("id")]
        public Category category { get; set; }

    }

    //Comme l'annotation de base key n'est pas recommandée j'ai utilisé FluentValidation: http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
    public class MetierValidator : AbstractValidator<Metier>
    {
        public MetierValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Le nom du métier est requis.").Length(0, 100);
            RuleFor(x => x.name).Must(BeUniqueUrl).WithMessage("Ce nom de métier existe déjà.");
        }

        private bool BeUniqueUrl(string name)
        {
            var _db = new MetierDbContext();
            if (_db.Metiers.SingleOrDefault(x => x.name == name) == null) return true;
            return false;
        }
    }

    //Gérer le lien vers la BDD: 
    // - http://stackoverflow.com/questions/18635050/proper-way-to-use-dbcontext-class-in-mvc
    // - http://mvc4beginner.com/Tutorial/Introducing-DBContext-&-DBSet.html
    // - http://www.codeproject.com/Articles/627704/Learning-MVC-Part-Creating-MVC-Application-with

    public class MetierDbContext : DbContext
    {
        public MetierDbContext()
            : base("DefaultConnection")
        { }

        public static MetierDbContext create()
        {
            return new MetierDbContext();
        }

        public DbSet<Metier> Metiers { get; set; }
    }
}
