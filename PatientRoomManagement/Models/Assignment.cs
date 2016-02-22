using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using PatientRoomManagement.DataLayer;

namespace PatientRoomManagement.Models
{
    public class Assignment
    {

        private Assignment()
        {
        }

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

        public static Assignment Create(Patient patient, ref Room room)
        {
            if (room.AvailableSpace == 0)
            {
                throw new InvalidOperationException($"Room #{room.Number} has no available beds. Please select an other room.");
            }

            if (!string.IsNullOrEmpty(room.Gender) &&
                !room.Gender.Equals(patient.Gender))
            {
                throw new InvalidOperationException($"Cannot assign {patient.Gender.ToLower()} patient to a {room.Gender.ToLower()} room");
            }

            var assignment = new Assignment()
            {
                PatientId = patient.Id,
                RoomId = room.Id,
                SignInDate = DateTime.Now,
            };

            if (string.IsNullOrEmpty(room.Gender))
            {
                room.Gender = patient.Gender;
            }

            return assignment;
        }
    }
}