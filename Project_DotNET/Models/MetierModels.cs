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
    [Validator(typeof(JobValidator))]
    public class Job
    {
        [Column(Order = 3)]
        public int Id { get; set; }

        [Required]
        public string JobName { get; set; }

        public string JobDesc { get; set; }

        public virtual Category Category { get; set; }

        public int CategoryId { get; set; }
    }

    //Comme l'annotation de base key n'est pas recommandée j'ai utilisé FluentValidation: http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
    public class JobValidator : AbstractValidator<Job>
    {
        public JobValidator()
        {
            RuleFor(x => x.JobName).NotEmpty().WithMessage("Le nom du métier est requis.").Length(2, 100);
            RuleFor(x => x.JobName).Must(BeUniqueName).WithMessage("Ce nom de métier existe déjà.");
            RuleFor(x => x.Category).NotNull().WithMessage("La catégorie est requise.");
        }

        private bool BeUniqueName(string Job)
        {
            var _db = new ApplicationDbContext();
            if (_db.Jobs.SingleOrDefault(x => x.JobName == Job) == null) return true;
            return false;
        }
    }

    //Gérer le lien vers la BDD: 
    // - http://stackoverflow.com/questions/18635050/proper-way-to-use-dbcontext-class-in-mvc
    // - http://mvc4beginner.com/Tutorial/Introducing-DBContext-&-DBSet.html
    // - http://www.codeproject.com/Articles/627704/Learning-MVC-Part-Creating-MVC-Application-with

    /*public class JobDbContext : DbContext
    {
        public JobDbContext()
            : base("NewCo")
        { }

        public static JobDbContext create()
        {
            return new JobDbContext();
        }

        public DbSet<Job> Jobs { get; set; }
    }*/
}
