using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseEntities;

namespace Cronus.Controllers
{
    public class HoursWorkedController : Controller
    {
        private CronusDatabaseEntities db = new CronusDatabaseEntities();

        // GET: HoursWorked
        public ActionResult Index()
        {
            var hoursworkeds = db.hoursworkeds.Include(h => h.activity).Include(h => h.employeetimeperiod).Include(h => h.project);
            return View(hoursworkeds.ToList());
        }

        // GET: HoursWorked/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hoursworked hoursworked = db.hoursworkeds.Find(id);
            if (hoursworked == null)
            {
                return HttpNotFound();
            }
            return View(hoursworked);
        }

        // GET: HoursWorked/Create
        public ActionResult Create()
        {
            ViewBag.Activity_activityID = new SelectList(db.activities, "activityID", "activityName");
            ViewBag.TimePeriod_employeeID = new SelectList(db.employeetimeperiods, "Employee_employeeID", "Employee_employeeID");
            ViewBag.Project_projectID = new SelectList(db.projects, "projectID", "projectName");
            return View();
        }

        // POST: HoursWorked/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "entryID,hours,date,comments,TimePeriod_employeeID,TimePeriod_periodEndDate,Activity_activityID,Project_projectID")] hoursworked hoursworked)
        {
            if (ModelState.IsValid)
            {
                db.hoursworkeds.Add(hoursworked);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Activity_activityID = new SelectList(db.activities, "activityID", "activityName", hoursworked.Activity_activityID);
            ViewBag.TimePeriod_employeeID = new SelectList(db.employeetimeperiods, "Employee_employeeID", "Employee_employeeID", hoursworked.TimePeriod_employeeID);
            ViewBag.Project_projectID = new SelectList(db.projects, "projectID", "projectName", hoursworked.Project_projectID);
            return View(hoursworked);
        }

        // GET: HoursWorked/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hoursworked hoursworked = db.hoursworkeds.Find(id);
            if (hoursworked == null)
            {
                return HttpNotFound();
            }
            ViewBag.Activity_activityID = new SelectList(db.activities, "activityID", "activityName", hoursworked.Activity_activityID);
            ViewBag.TimePeriod_employeeID = new SelectList(db.employeetimeperiods, "Employee_employeeID", "Employee_employeeID", hoursworked.TimePeriod_employeeID);
            ViewBag.Project_projectID = new SelectList(db.projects, "projectID", "projectName", hoursworked.Project_projectID);
            return View(hoursworked);
        }

        // POST: HoursWorked/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "entryID,hours,date,comments,TimePeriod_employeeID,TimePeriod_periodEndDate,Activity_activityID,Project_projectID")] hoursworked hoursworked)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hoursworked).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Activity_activityID = new SelectList(db.activities, "activityID", "activityName", hoursworked.Activity_activityID);
            ViewBag.TimePeriod_employeeID = new SelectList(db.employeetimeperiods, "Employee_employeeID", "Employee_employeeID", hoursworked.TimePeriod_employeeID);
            ViewBag.Project_projectID = new SelectList(db.projects, "projectID", "projectName", hoursworked.Project_projectID);
            return View(hoursworked);
        }

        // GET: HoursWorked/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hoursworked hoursworked = db.hoursworkeds.Find(id);
            if (hoursworked == null)
            {
                return HttpNotFound();
            }
            return View(hoursworked);
        }

        // POST: HoursWorked/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hoursworked hoursworked = db.hoursworkeds.Find(id);
            db.hoursworkeds.Remove(hoursworked);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddHours(hoursworked entry)
        {
            var result = db.hoursworkeds.Find(entry.entryID);
            if (result != null)
            {
                //result.project = entry.project; result.activity = entry.activity;
                result.Project_projectID = entry.Project_projectID; result.Activity_activityID = entry.Activity_activityID;
                result.hours = entry.hours; result.comments = entry.comments;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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
