using FluentValidation;
using Project_DotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Utils
{
    public class CreateJobVMValidator : AbstractValidator<CreateJobViewModel>
    {
        public CreateJobVMValidator()
        {
            var db = new ApplicationDbContext();
            RuleFor(x => x.Name).Must((x, name) => { return notExists(x.Name, x.SelectedCategory); }).WithMessage("Ce nom existe déjà.");
        }

        public bool notExists(string name, int companyId)
        {
            var _db = new ApplicationDbContext();

            var nb = _db
                .Jobs
                .Select(x => x)
                .Where(x => x.JobName == name && x.CategoryId == companyId )
                .Count();
            return nb == 0 ? true : false;
        }
    }
}
