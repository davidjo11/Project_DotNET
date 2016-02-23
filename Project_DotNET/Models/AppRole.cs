using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_DotNET.Models
{
    public class AppRole
    {
        
        public AppRole()
        {
            this.AppRoles = new List<AvailableRole>();
        }
        public int AppRoleId { get; set; }

        public string AppRoleName { get; set; }

        public string AppRoleDesc { get; set; }

        public virtual ICollection<AvailableRole> AppRoles { get; set; }

        public bool addRole(AvailableRole Role)
        {
            var size = this.AppRoles.Count;
            this.AppRoles.Add(Role);
            if (size + 1 == this.AppRoles.Count)
            {
                this.AppRoles.OrderBy(x => x.AvailableRoleName);
                return true;
            }
            return false;
        }
    }

}