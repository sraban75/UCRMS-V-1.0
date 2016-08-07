using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class ViewResult
    {
        public int ViewResultId { get; set; }
        public string CourseCode { set; get; }
        public string CourseName { set; get; }
        public string Grade { set; get; }
    }
}