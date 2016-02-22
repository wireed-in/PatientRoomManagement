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

namespace PatientRoomManagement.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Patient
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FNameSortParm = String.IsNullOrEmpty(sortOrder) ? "fname_desc" : "";
            ViewBag.LNameSortParm = sortOrder == "lname" ? "lname_desc" : "lname";
            ViewBag.DobSortParm = sortOrder == "dob" ? "dob_desc" : "dob";
            ViewBag.MrnSortParm = sortOrder == "mrn" ? "mrn_desc" : "mrn";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var patients = db.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                patients = patients.Where(p => p.FirstName.Contains(searchString) || p.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "fname_desc":
                    patients = patients.OrderByDescending(p => p.FirstName);
                    break;
                case "lname":
                    patients = patients.OrderBy(p => p.LastName);
                    break;
                case "lname_desc":
                    patients = patients.OrderByDescending(p => p.LastName);
                    break;
                case "dob":
                    patients = patients.OrderBy(p => p.Dob);
                    break;
                case "dob_desc":
                    patients = patients.OrderByDescending(p => p.Dob);
                    break;
                case "mrn":
                    patients = patients.OrderBy(p => p.Mrn);
                    break;
                case "mrn_desc":
                    patients = patients.OrderByDescending(p => p.Mrn);
                    break;
                default:
                    patients = patients.OrderBy(p => p.FirstName);
                    break;
            }

            // Set the number of list items you would like to see per page.
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            
            return View(patients.ToPagedList(pageNumber, pageSize));
        }

        // GET: Patient/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Patient patient = db.Patients.Find(id);

            if (patient == null)
            {
                return HttpNotFound();
            }

            patient.AuditLogs = db.AuditLog.Where(i => i.RecordId == patient.Id.ToString()).OrderByDescending(x => x.EventDateUTC).ToList();

            return View(patient);
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Gender,Dob,Mrn")] Patient patient)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Patients.Add(patient);
                    db.SaveChanges(User.Identity.Name);
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dataEx)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(patient);
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patientToUpdate = db.Patients.Find(id);

            if (TryUpdateModel(patientToUpdate, "", new string[] {"FirstName", "LastName", "Gender", "Dob", "Mrn"}))
            {
                try
                {
                    db.SaveChanges(User.Identity.Name);
                    return RedirectToAction("Index");
                }
                catch (DataException dataEx)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return View(patientToUpdate);
        }

        // GET: Patient/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges(User.Identity.Name);
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
