using FluentValidation;
using Project_DotNET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Utils
{
    public class CreateRoleVMValidator : AbstractValidator<CreateAvailableRoleViewModel>
    {
        public CreateRoleVMValidator()
        {
            var db = new ApplicationDbContext();
            RuleFor(x => x.Name).Must((x, name) => { return notExists(x.Name);}).WithMessage("Ce rôle existe déjà.");
        }

        public bool notExists(string name)
        {
            var _db = new ApplicationDbContext();

            var nb = _db
                .AvailableRoles
                .Select(x => x)
                .Where(x => x.AvailableRoleName == name)
                .Count();
            return nb == 0 ? true : false;
        }
    }
}
