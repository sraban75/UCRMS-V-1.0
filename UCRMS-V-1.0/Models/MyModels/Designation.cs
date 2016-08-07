using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class Designation
    {
        public int DesignationId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}