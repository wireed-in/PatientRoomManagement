﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TrackerEnabledDbContext.Common.Models;

namespace PatientRoomManagement.Models
{
    [TrackChanges]
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Room Number")]
        public int Number { get; set; }

        [Required]
        [Display(Name = "Number Of Beds")]
        public int NumberOfBeds { get; set; }

        public string Gender { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

        public virtual ICollection<Patient> Patients
        {
            get { return Assignments?.Where(a => a.SignOutDate == null).Select(x => x.Patient).ToList(); }
            set { }
        }

        public virtual int? AvailableSpace
        {
            get
            {
                var availableSpace = NumberOfBeds - Assignments.Count(a => a.SignOutDate == null);
                return availableSpace;
            }
        }

        [NotMapped]
        public virtual List<AuditLog> AuditLogs { get; set; }
    }
}