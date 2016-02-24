using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using PatientRoomManagement.DataLayer;
using PatientRoomManagement.Utilities;

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

        /// <summary>
        /// This method is the only way to create an assignment. Initializing assignment without invoking this method is not allowed
        /// since there are some room/available space checks that are necessary upon assignment.
        /// </summary>
        /// <param name="patient">Patients needs to be passed since this method will check to ensure that patient is eligible for the room that he/she is being assigned</param>
        /// <param name="room">Room needs to be passed as a reference since this method will assign gender to a room if its the first assignment for that room.</param>
        /// <returns></returns>
        public static Assignment Create(Patient patient, Room room)
        {
            if (room.AvailableSpace == 0)
            {
                throw new IllegalAssignmentException($"Room #{room.Number} has no available beds. Please select an other room.");
            }

            if (!string.IsNullOrEmpty(room.Gender) &&
                !room.Gender.Equals(patient.Gender))
            {
                throw new IllegalAssignmentException($"Cannot assign {patient.Gender.ToLower()} patient to a {room.Gender.ToLower()} room");
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