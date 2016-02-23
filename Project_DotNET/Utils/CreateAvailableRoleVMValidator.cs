using FluentValidation;
using Project_DotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Utils
{
    public class CreateAvailableRoleVMValidator : AbstractValidator<CreateAvailableRoleViewModel>
    {
        public CreateAvailableRoleVMValidator()
        {
            var db = new ApplicationDbContext();
            RuleFor(x => x.Name).Must((x, name) => { return notExists(x.Name); }).WithMessage("Ce nom existe déjà.");
        }

        public bool notExists(string name)
        {
            var _db = new ApplicationDbContext();

            var nb = _db
                .AppRoles
                .Select(x => x)
                .Where(x => x.AppRoleName == name )
                .Count();
            return nb == 0 ? true : false;
        }
    }
}
