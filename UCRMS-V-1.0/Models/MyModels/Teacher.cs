using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name Must Be Three (3) To Fifty (50) Character Long.")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        [Remote("IsEmailAvailable", "Teachers", ErrorMessage = "Email Already Exist.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact No. is Required")]
        [Display(Name = "Contact No.")]
        [StringLength(20,MinimumLength = 10,ErrorMessage = "Contact Number Must Be Ten (10) To Twenty (20) Digit Long")]
        public string ContactNo { get; set; }

        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Credit is Required")]
        [Display(Name = "Credit to be taken")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Credit Must Be Positive Number")]
        public double TakenCredit { get; set; }

        public virtual Designation Designation { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<AssignCourse> AssignCourses { get; set; }
    }
}