using FluentValidation;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_DotNET.Models
{
    public class Job
    {
        //[Column(Order = 3)]
        public int JobId { get; set; }

        public string JobName { get; set; }

        public string JobDesc { get; set; }

        public virtual Category Category { get; set; }

        public int CategoryId { get; set; }
    }

}
