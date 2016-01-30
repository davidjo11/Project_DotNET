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
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Pour que la BDD génère auto. un ID unique.
        public int id { get; }

        [Required]
        public string name { get; set; }

        public string description { get; set; }

        public virtual List<Metier> metiers { get; set; }
    }

    //Comme l'annotation de base key n'est pas recommandée j'ai utilisé FluentValidation: http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Le nom d'une categorie est requis.").Length(0, 100);
            RuleFor(x => x.name).Must(BeUniqueUrl).WithMessage("Ce nom de categorie existe déjà.");
        }

        private bool BeUniqueUrl(string name)
        {
            var _db = new CategoryDbContext();
            if (_db.Categories.SingleOrDefault(x => x.name == name) == null) return true;
            return false;
        }
    }

    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext()
            : base("DefaultConnection")
        { }

        public static CategoryDbContext create()
        {
            return new CategoryDbContext();
        }

        public DbSet<Category> Categories { get; set; }
    }
}
