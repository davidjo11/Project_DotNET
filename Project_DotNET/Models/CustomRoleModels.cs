using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_DotNET.Models
{
    public class CustomRole
    {
        public CustomRole()
        {
            //this.Roles = new List<Role>();
            this.RolesInc = new RolesInc();
        }

        public int CustomRoleId { get; set; }

        public string RoleName { get; set; }

        //Incompatibilités
        [InverseProperty("BelongsTo")] 
        public int RolesIncId { get; set; }

        public virtual RolesInc RolesInc { get; set; }

        public string RoleDesc { get; set; }

        /*public bool addIncompatibilite(Role role)
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

        public bool removeIncompatibilite(string role)
        {
            var size = this.Roles.Count;

            var exists = this.Roles
                .Select(x => x)
                .Where(x => x.RoleName == role)
                .First();

            if (exists == null)
                return false;

            this.Roles.Remove(exists);
            return size - 1 == this.Roles.Count;
        }*/
    }
}