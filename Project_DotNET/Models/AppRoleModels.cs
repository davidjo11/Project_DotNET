using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Models
{
    public class AppRole
    {
        public AppRole()
        {
            this.AppRoles = new List<AvailableRole>();
        }
        public int AppRoleId { get; set; }

        public virtual ICollection<AvailableRole> AppRoles { get; set; }

        public bool addRole(AvailableRole Role)
        {
            var size = this.AppRoles.Count;
            this.AppRoles.Add(Role);
            this.AppRoles.OrderBy(x => x.AvailableRoleName);
            return size + 1 == this.AppRoles.Count;
        }

        public bool removeRole(AvailableRole Role)
        {
            var size = this.AppRoles.Count;
            this.AppRoles.Remove(Role);
            this.AppRoles.OrderBy(x => x.AvailableRoleName);
            return size - 1 == this.AppRoles.Count ;
        }
    }

    public class AppRoleValidator : AbstractValidator<AppRole>
    {
        public AppRoleValidator()
        {
            RuleFor(x => x.AppRoles).NotNull().WithMessage("Il n'a pas de rôle a valider.");
            //Vérifie qu'il n'y a pas d'incompatibilités dans la liste des rôles occupés par le User
            RuleFor(x => x.AppRoles)
                .Must(x => { return !containsIncompatibilities(x); })
                .WithMessage("Certains rôles sont incompatibles:\n"
                            + "\t- Responsable technique et Référent technique"
                            + "\t- Responsable fonctionnel et Référent fonctionnel"
                            + "Ces couples étant incompatibles, vous ne pouvez pas les avoir occupés en mettant temps.");

            //pas de doublon 
            RuleFor(x => x).Must(x => !isDuplicate(x)).WithMessage("Ce rôle a déja été ajouté.");

        }
        
        private bool isDuplicate(AppRole ar)  {

            var duplicate = ar.AppRoles.GroupBy(x => x).Where(group => group.Count() > 1).Select(group => group.Key);
            return duplicate.Count()>1;
        }

        private bool containsIncompatibilities(ICollection<AvailableRole> roles)
        {

            var respFct = roles.Where(x => x.AvailableRoleName.Equals("Responsable fonctionnel"));
            var respTech = roles.Where(x => x.AvailableRoleName.Equals("Responsable technique"));
            var refFct = roles.Where(x => x.AvailableRoleName.Equals("Référent fonctionnel"));
            var refTech = roles.Where(x => x.AvailableRoleName.Equals("Référent technique"));

            if (respFct.Count()>0 && refFct.Count() > 0) { return true; }
            if (respTech.Count() > 0 && refTech.Count() > 0) { return true; }
            return false;

        }
    }
}
