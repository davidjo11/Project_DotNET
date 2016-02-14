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
    [Validator(typeof(CompanyValidator))]
    public class Company
    {
        //[Column(Order = 2)]
        public int CompanyId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string country { get; set; }

        [Required]
        public string city { get; set; }

        //Comme l'annotation de base key n'est pas recommandée j'ai utilisé FluentValidation: http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
        public class CompanyValidator : AbstractValidator<Company>
        {
            public CompanyValidator()
            {
                RuleFor(x => x.CompanyName).NotEmpty().WithMessage("Le nom du métier est requis.").Length(2, 100);
                RuleFor(x => x.country).NotEmpty().WithMessage("Le nom du pays est requis.").Length(2, 100);
                RuleFor(x => x.city).NotEmpty().WithMessage("Le nom de la ville est requis.").Length(2, 100);
            }

            private bool BeUniqueName(string name)
            {
                var _db = new ApplicationDbContext();
                if (_db.Companies.SingleOrDefault(x => x.CompanyName == name) == null) return true;
                return false;
            }
        }

    }
    /*public class CompanyDbContext : DbContext
    {
        public CompanyDbContext()
            : base("NewCo")
        { }

        public static CompanyDbContext create()
        {
            return new CompanyDbContext();
        }

        public DbSet<Company> Companies { get; set; }
    }*/
}
