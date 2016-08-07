using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class Grade
    {
        public int GradeId { get; set; }

        [DisplayName("Grade")]
        public string Name { get; set; }

        public virtual ICollection<SaveResult> StudentResults { get; set; }
    }
}