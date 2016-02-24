using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PatientRoomManagement.DataLayer;
using PatientRoomManagement.Models;
using PatientRoomManagement.Utilities;
using PatientRoomManagement.ViewModels;

namespace PatientRoomManagement.Controllers
{
    [Authorize]
    public class AssignmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Assignment
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // Following lines of code handle the ordering the record in that column.
            // Viewbags are used to keep track of the current order and enforce the
            // same order through pagination.
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.RoomNumberSortParm = sortOrder == "roomnumber" ? "roomnumber_desc" : "roomnumber";
            ViewBag.SigninSortParm = sortOrder == "signin" ? "signin_desc" : "signin";
            ViewBag.SignoutSortParm = sortOrder == "signout" ? "signout_desc" : "signout";

            // reset the pagination once search was performed
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            // Keep track of current filter for pagination
            ViewBag.CurrentFilter = searchString;

            var assignments = db.Assignments.Include(a => a.Patient).Include(a => a.Room);

            if (!string.IsNullOrEmpty(searchString))
            {
                assignments = assignments.Where(a => a.Patient.FullName.Contains(searchString) || a.Room.Number.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    assignments = assignments.OrderByDescending(a => a.Patient.FirstName);
                    break;
                case "roomnumber":
                    assignments = assignments.OrderBy(a => a.Room.Number);
                    break;
                case "roomnumber_desc":
                    assignments = assignments.OrderByDescending(a => a.Room.Number);
                    break;
                case "signin":
                    assignments = assignments.OrderBy(a => a.SignInDate);
                    break;
                case "signin_desc":
                    assignments = assignments.OrderByDescending(a => a.SignInDate);
                    break;
                case "signout":
                    assignments = assignments.OrderBy(a => a.SignOutDate);
                    break;
                case "signout_desc":
                    assignments = assignments.OrderByDescending(a => a.SignOutDate);
                    break;
                default:
                    assignments = assignments.OrderBy(a => a.Patient.FirstName);
                    break;
            }

            // Set the number of list items you would like to see per page.
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(assignments.ToPagedList(pageNumber, pageSize));
        }

        // GET: Assignment/Create
        public ActionResult Create()
        {
            var assignmentViewModel = new AssignmentViewModel()
            {
                Patients = new SelectList(db.Patients, "Id", "FirstName"),
                Rooms = new SelectList(db.Rooms, "Id", "Number"),
            };

            return View(assignmentViewModel);
        }

        // POST: Assignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatientId, RoomId")] AssignmentViewModel assignmentViewModel)
        {
            var patient = db.Patients.Find(assignmentViewModel.PatientId);
            var room = db.Rooms.Find(assignmentViewModel.RoomId);

            // Assignment creation needs to be in try catch to catch illegal assignments.
            try
            {
                var assignment = Assignment.Create(patient, room);

                if (ModelState.IsValid)
                {
                    db.Assignments.Add(assignment);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (IllegalAssignmentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (DataException dataEx)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            assignmentViewModel = new AssignmentViewModel()
            {
                Patients = new SelectList(db.Patients, "Id", "FirstName"),
                Rooms = new SelectList(db.Rooms, "Id", "Number"),
            };

            return View(assignmentViewModel);
        }

        // GET: Assignment/Signout/5
        public ActionResult Signout(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var assignments = db.Assignments.Include(a => a.Patient).Include(a => a.Room);
            Assignment assignment = assignments.FirstOrDefault(a => a.Id == id);
            
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignment/Delete/5
        [HttpPost, ActionName("Signout")]
        [ValidateAntiForgeryToken]
        public ActionResult SignoutConfirmed(int id)
        {
            try
            {
                var assignments = db.Assignments.Include(a => a.Room);
                Assignment assignment = assignments.FirstOrDefault(a => a.Id == id);

                // Signing out a patient needs to reset the room gender if the room
                // becomes fully available for anyone.
                // TO-DO move this to a method where this is enforced through common
                // signout process and not need to be checked by developer where signout
                // is performed
                if (assignment != null)
                {
                    assignment.SignOutDate = DateTime.Now;

                    if (assignment.Room.Assignments.All(a => a.SignOutDate.HasValue))
                    {
                        assignment.Room.Gender = String.Empty;
                    }
                }

                db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
