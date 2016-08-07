using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class AllocateClassRoom
    {
        public int AllocateClassRoomId { get; set; }

        [Required(ErrorMessage = "Please Select A Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Select A Course")]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Please Select A Room")]
        public int RoomId { get; set; }
        [Required(ErrorMessage = "Please Select A Day")]
        public int DayId { get; set; }

        [Required(ErrorMessage = "Please Set The Class Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public DateTime From { get; set; }

        [Required(ErrorMessage = "Please Set The Class Time")]
        [DataType(DataType.Time)]

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm}")]
        public DateTime To { get; set; }

        public virtual Department Department { get; set; }
        public virtual Course Course { get; set; }
        public virtual Room Room { get; set; }
        public virtual Day Day { get; set; }
        public bool Status { get; set; }
    }
}