using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Code is Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Code must be at least five(5) character long")]
        [Index(IsUnique = true)]
        [Remote("IsCodeAvailble", "Courses", ErrorMessage = "SORRY!! Code Already Exist.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        [Remote("IsNameAvailble", "Courses", ErrorMessage = "SORRY!! Name Already Exist.")]
        public string Name { get; set; }

        [Range(0.5, 5, ErrorMessage = "OOPS!! Credit can't be less than 0.5 and more than 5.0")]
        public double Credit { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int DepartmentId { get; set; }
        public int SemesterId { get; set; }

        public virtual Department Department { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual int? AssignTo { get; set; }
        public virtual string Grade { get; set; }
        public bool Status { get; set; }

        public virtual int AlloRoomId { get; set; }
        public virtual ICollection<AssignCourse> AssignCourses { get; set; }
        public virtual ICollection<AllocateClassRoom> AllocateClassRooms { get; set; }
        public virtual ICollection<EnrollCourse> EnrollCourses { get; set; }
        public virtual ICollection<SaveResult> SaveResults { get; set; }
        public virtual ICollection<ViewResult> ViewResults { get; set; }
    }
}