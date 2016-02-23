using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatientRoomManagement.Models;

namespace PatientRoomManagement.ViewModels
{
    public class DashboardViewModel
    {
        public int PatientId { get; set; }
        public int RoomId { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public ICollection<Patient> Patients { get; set; }
    }
}