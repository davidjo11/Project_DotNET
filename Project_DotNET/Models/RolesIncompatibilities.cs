using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DotNET.Models
{
    public class RolesInc
    {
        public RolesInc()
        {
            this.Roles = new List<CustomRole>();
        }

        public int RolesIncId { get; set; }

        public virtual ICollection<CustomRole> Roles { get; set; }

        public bool addIncompatibilite(CustomRole role)
        {
            var size = this.Roles.Count;

            var exists = this.Roles
                .Select(x => x)
                .Where(x => x.RoleName == role.RoleName)
                .First();

            if (exists != null)
                return false;

            this.Roles.Add(role);
            return this.Roles.Count == size + 1;
        }

        public bool removeIncompatibilite(CustomRole role)
        {
            var size = this.Roles.Count;

            var exists = this.Roles
                .Select(x => x)
                .Where(x => x.RoleName == role.RoleName)
                .First();

            if (exists == null)
                return false;

            this.Roles.Remove(exists);
            return size - 1 == this.Roles.Count;
        }
    }
}