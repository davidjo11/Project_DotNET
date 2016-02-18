using FluentValidation;
using Project_DotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Utils
{
    public class CreateRoleVMValidator : AbstractValidator<CreateRoleViewModel>
    {
        public CreateRoleVMValidator()
        {
            RuleFor(x => x.Name).Must((x, name) => { return notExists(x.Name); }).WithMessage("Ce rôle existe déjà.");
        }

        private bool notExists(string name)
        {
            var _db = new ApplicationDbContext();

            var nb = _db
                .CustomRoles
                .Select(x => x)
                .Where(x => x.RoleName == name)
                .Count();
            return nb == 0 ? true : false;
        }
    }
}
