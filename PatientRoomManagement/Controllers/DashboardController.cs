using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PatientRoomManagement.DataLayer;
using PatientRoomManagement.ViewModels;
using WebGrease.Css.Extensions;

namespace PatientRoomManagement.Controllers
{
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashboard
        public ActionResult Index()
        {
            var unassignedPatients = db.Patients.Where(p => p.Assignments.Count == 0 || p.Assignments.All(a => a.SignOutDate.HasValue));

            var availableRooms = db.Rooms.Where(r => r.Assignments.Count == 0 || r.NumberOfBeds > r.Assignments.Count(a => !a.SignOutDate.HasValue));

            var possibleAssignments = new DashboardViewModel()
            {
                Rooms = availableRooms.ToList(),
                Patients = unassignedPatients.ToList()
            };

            return View(possibleAssignments);
        }
    }
}