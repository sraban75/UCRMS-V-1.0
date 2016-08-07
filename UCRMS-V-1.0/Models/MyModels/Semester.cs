using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class Semester
    {
        public int SemesterId { get; set; }

        [DisplayName("Semester")]
        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}