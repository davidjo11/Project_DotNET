using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Models
{
    class User
    {
        public int id { get; }

        public String firstName { get; set; }

        public String lastName { get; set; }

        public DateTime birthday { get; set; }

        public DateTime firstDay { get; set; }
    }
}
