using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatientRoomManagement.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int PatientId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Sign-in Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime SignInDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Sign-out Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? SignOutDate { get; set; }

        public virtual Room Room { get; set; }
        public virtual Patient Patient { get; set; }
    }
}