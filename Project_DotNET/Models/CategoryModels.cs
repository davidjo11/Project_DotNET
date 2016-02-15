using FluentValidation;
using FluentValidation.Attributes;
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
    [Validator(typeof(CategoryValidator))]
    public class Category
    {
        public Category()
        {
            this.Jobs = new List<Job>();
        }
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDesc { get; set; }

        public virtual ICollection<Job> Jobs { get; set; }

        public bool addJob(Job Job)
        {
            var size = this.Jobs.Count;
            this.Jobs.Add(Job);
            if (size + 1 == this.Jobs.Count)
            {
                this.Jobs.OrderBy(x => x.JobName);
                return true;
            }
            return false;
        }
    }

    //Comme l'annotation de base key n'est pas recommandée j'ai utilisé FluentValidation: http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Le nom d'une categorie est requis.").Length(0, 100);
            RuleFor(x => x.CategoryName).Must(BeUniqueName).WithMessage("Ce nom de categorie existe déjà.");
        }

        private bool BeUniqueName(string name)
        {
            var _db = new ApplicationDbContext();
            if (_db.Categories.SingleOrDefault(x => x.CategoryName == name) == null) return true;
            return false;
        }
    }

    /*public class CategoryDbContext : DbContext
    {
        public CategoryDbContext()
            : base("NewCo")
        { }

        public static CategoryDbContext create()
        {
            return new CategoryDbContext();
        }

        public DbSet<Category> Categories { get; set; }
    }*/
}
