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
    public class RoomController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rooms
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            // Following lines of code handle the ordering the record in that column.
            // Viewbags are used to keep track of the current order and enforce the
            // same order through pagination.
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NumberSortParm = String.IsNullOrEmpty(sortOrder) ? "number_desc" : "";
            ViewBag.BedsSortParm = sortOrder == "beds" ? "beds_desc" : "beds";
            ViewBag.GenderSortParm = sortOrder == "gender" ? "gender_desc" : "gender";

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

            var rooms = db.Rooms.Include(r => r.Assignments);

            if (!string.IsNullOrEmpty(searchString))
            {
                rooms = rooms.Where(r => searchString.Equals(r.Number.ToString()));
            }

            switch (sortOrder)
            {
                case "number_desc":
                    rooms = rooms.OrderByDescending(r => r.Number);
                    break;
                case "beds":
                    rooms = rooms.OrderBy(r => r.NumberOfBeds);
                    break;
                case "beds_desc":
                    rooms = rooms.OrderByDescending(r => r.NumberOfBeds);
                    break;
                case "gender":
                    rooms = rooms.OrderBy(r => r.Gender);
                    break;
                case "gender_desc":
                    rooms = rooms.OrderByDescending(r => r.Gender);
                    break;
                default:
                    rooms = rooms.OrderBy(r => r.Number);
                    break;
            }

            // Set the number of list items you would like to see per page.
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(rooms.ToPagedList(pageNumber, pageSize));
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Room room = db.Rooms.Include(r => r.Patients).FirstOrDefault(r => r.Id == id);

            if (room == null)
            {
                return HttpNotFound();
            }

            room.AuditLogs = db.AuditLog.Where(i => i.RecordId == room.Id.ToString()).OrderByDescending(x => x.EventDateUTC).ToList();

            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Number,NumberOfBeds,Gender")] Room room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Rooms.Add(room);
                    db.SaveChanges(User.Identity.Name);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Edit/5
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

            var roomToUpdate = db.Rooms.Find(id);

            if (TryUpdateModel(roomToUpdate, "", new string[] {"Number", "NumberOfBeds"}))
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

            return View(roomToUpdate);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
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
