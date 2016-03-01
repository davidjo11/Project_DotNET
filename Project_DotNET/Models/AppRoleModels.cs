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
            RuleFor(x =>x.AppRoles).NotNull().WithMessage("Il n'a pas de rôle a valider.");
            //Vérifie qu'il n'y a pas d'incompatibilités dans la liste des rôles occupés par le User
            RuleFor(x => x.AppRoles)
                .Must(x => { return !containsIncompatibilities(x); })
                .WithMessage("Certains rôles sont incompatibles:\n"
                            + "\t- Responsable technique et Référent technique"
                            + "\t- Responsable fonctionnel et Référent fonctionnel"
                            + "Ces couples étant incompatibles, vous ne pouvez pas les avoir occupés en mettant temps.");
        }

        private bool containsIncompatibilities(ICollection<AvailableRole> roles)
        {

            var respFct = roles.Where(x => x.AvailableRoleName == "Responsable fonctionnel");
            var respTech = roles.Where(x => x.AvailableRoleName == "Responsable technique");
            var refFct = roles.Where(x => x.AvailableRoleName == "Référent fonctionnel");
            var refTech = roles.Where(x => x.AvailableRoleName == "Référent technique");
            var t1 = respFct.GetEnumerator().Current;
            var t2 = respTech.GetEnumerator().Current;
            var t3 = refFct.GetEnumerator().Current;
            var t4 = refTech.GetEnumerator().Current;
            var toto = (respFct.GetEnumerator().Current != null && refFct.GetEnumerator().Current != null) || (respTech.GetEnumerator().Current != null && refTech.GetEnumerator().Current != null);

            return (respFct.GetEnumerator().Current != null && refFct.GetEnumerator().Current != null) || (respTech.GetEnumerator().Current != null && refTech.GetEnumerator().Current != null) ;
        }
    }
}
