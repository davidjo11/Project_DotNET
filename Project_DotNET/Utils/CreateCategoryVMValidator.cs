using FluentValidation;
using Project_DotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Utils
{
    public class CreateCategoryVMValidator : AbstractValidator<CreateCategoryViewModel>
    {
        public CreateCategoryVMValidator()
        {
            CascadeMode = CascadeMode.Continue;
            RuleFor(x => x.Name).Must((x, name) => { return notExists(x.Name); }).WithMessage("Cette catégorie existe déjà.");
        }

        private bool notExists(string name)
        {
            var _db = new ApplicationDbContext();

            var nb = _db
                .Categories
                .Select(x => x)
                .Where(x => x.CategoryName == name)
                .Count();
            return nb == 0 ? true : false;
        }
    }
}
