using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zero_Hunger.EF;

namespace Zero_Hunger.Models
{
    public class info_class
    {
        public List<Employee> emp { get; set; }

        public List<Donation> don { get; set; }

        public List<Collection> col { get; set; }

        public List<Location> loc { get; set; }
    }
}