using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class EnrollCourse
    {
        public int EnrollCourseId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}