using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class Department
    {
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Code is Required")]
        [StringLength(7, MinimumLength = 2, ErrorMessage = "Code must be two(2) to seven(7) character long")]
        [Index(IsUnique = true)]
        [Remote("IsCodeAvailble", "Departments", ErrorMessage = "Code Already Exist.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        [Remote("IsNameAvailble", "Departments", ErrorMessage = "Name Already Exist.")]
        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<AssignCourse> AssignCourses { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<AllocateClassRoom> AllocateClassRooms { get; set; }
    }
}