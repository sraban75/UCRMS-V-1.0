using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class Day
    {
        public int DayId { get; set; }

        [DisplayName("Day")]
        public string Name { get; set; }
        public virtual ICollection<AllocateClassRoom> AllocateClassRooms { get; set; }
    }
}