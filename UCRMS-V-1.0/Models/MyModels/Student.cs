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
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(40,MinimumLength = 3, ErrorMessage = "Name must be Three (3) to Forty (40) Character Long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [StringLength(35)]
        [Index(IsUnique = true)]
        [Remote("IsEmailAvailable", "Students", ErrorMessage = "Email Already Exist.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date is Required")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }

        [DisplayName("Contact No")]
        [Required(ErrorMessage = "Contact is Required")]
        [StringLength(20,MinimumLength = 10, ErrorMessage = "Number Must be Ten (10) to Twenty (20) Digit")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Please Type Your Address")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Address Maximum Five Hundred (500) Character Long")]
        public string Address { get; set; }
        public int DepartmentId { get; set; }

        [Column(TypeName = "DateTime2")]
        public virtual DateTime ResultDate { get; set; }


        public virtual string RegNo { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<EnrollCourse> EnrollCourses { get; set; }
        public virtual ICollection<SaveResult> SaveResults { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}