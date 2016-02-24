using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Models
{
    public class AvailableRole
    {
        //[Column(Order = 3)]
        //Key
        public int AvailableRoleId { get; set; }

        public string AvailableRoleName { get; set; }

        public string AvailableRoleDesc { get; set; }
    }
}
