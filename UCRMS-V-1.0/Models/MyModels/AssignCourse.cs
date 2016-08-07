using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class AssignCourse
    {
        public int AssignCourseId { get; set; }
        public int DepartmentId { get; set; }
        public int TeacherId { get; set; }
        public double CreditToBeTaken { get; set; }
        public double RemainigCredit { get; set; }
        public int CourseId { get; set; }

        public virtual Department Department { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Course Course { get; set; }

        public bool Status { get; set; }
    }
}