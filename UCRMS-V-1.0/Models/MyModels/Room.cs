using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace UCRMS_V_1._0.Models.MyModels
{
    public class Room
    {
        public int RoomId { get; set; }

        [DisplayName("Room")]
        public string Name { get; set; }
        public virtual ICollection<AllocateClassRoom> AllocateClassRooms { get; set; }
    }
}