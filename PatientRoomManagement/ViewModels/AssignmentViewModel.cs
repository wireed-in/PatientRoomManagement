using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatientRoomManagement.Models;

namespace PatientRoomManagement.ViewModels
{
    public class AssignmentViewModel
    {
        public int PatientId { get; set; }
        public int RoomId { get; set; }
        public SelectList Rooms { get; set; }
        public SelectList Patients { get; set; }
    }
}