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
    public class TimePeriodByEmployeeController : Controller
    {
        private CronusDatabaseEntities db = new CronusDatabaseEntities();

        // GET: TimePeriodByEmployee
        public ActionResult Index()
        {
            var employeetimeperiods = db.employeetimeperiods.Include(e => e.employee).Include(e => e.timeperiod);
            return View(employeetimeperiods.ToList());
        }

        // GET: TimePeriodByEmployee/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employeetimeperiod employeetimeperiod = db.employeetimeperiods.Find(id);
            if (employeetimeperiod == null)
            {
                return HttpNotFound();
            }
            return View(employeetimeperiod);
        }

        // GET: TimePeriodByEmployee/Create
        public ActionResult Create()
        {
            ViewBag.Employee_employeeID = new SelectList(db.employees, "employeeID", "employeeFirstName");
            ViewBag.TimePeriod_periodEndDate = new SelectList(db.timeperiods, "periodEndDate", "periodEndDate");
            return View();
        }

        // POST: TimePeriodByEmployee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Employee_employeeID,TimePeriod_periodEndDate,isApproved")] employeetimeperiod employeetimeperiod)
        {
            if (ModelState.IsValid)
            {
                db.employeetimeperiods.Add(employeetimeperiod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Employee_employeeID = new SelectList(db.employees, "employeeID", "employeeFirstName", employeetimeperiod.Employee_employeeID);
            ViewBag.TimePeriod_periodEndDate = new SelectList(db.timeperiods, "periodEndDate", "periodEndDate", employeetimeperiod.TimePeriod_periodEndDate);
            return View(employeetimeperiod);
        }

        // GET: TimePeriodByEmployee/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employeetimeperiod employeetimeperiod = db.employeetimeperiods.Find(id);
            if (employeetimeperiod == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employee_employeeID = new SelectList(db.employees, "employeeID", "employeeFirstName", employeetimeperiod.Employee_employeeID);
            ViewBag.TimePeriod_periodEndDate = new SelectList(db.timeperiods, "periodEndDate", "periodEndDate", employeetimeperiod.TimePeriod_periodEndDate);
            return View(employeetimeperiod);
        }

        // POST: TimePeriodByEmployee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Employee_employeeID,TimePeriod_periodEndDate,isApproved")] employeetimeperiod employeetimeperiod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeetimeperiod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employee_employeeID = new SelectList(db.employees, "employeeID", "employeeFirstName", employeetimeperiod.Employee_employeeID);
            ViewBag.TimePeriod_periodEndDate = new SelectList(db.timeperiods, "periodEndDate", "periodEndDate", employeetimeperiod.TimePeriod_periodEndDate);
            return View(employeetimeperiod);
        }

        // GET: TimePeriodByEmployee/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            employeetimeperiod employeetimeperiod = db.employeetimeperiods.Find(id);
            if (employeetimeperiod == null)
            {
                return HttpNotFound();
            }
            return View(employeetimeperiod);
        }

        // POST: TimePeriodByEmployee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            employeetimeperiod employeetimeperiod = db.employeetimeperiods.Find(id);
            db.employeetimeperiods.Remove(employeetimeperiod);
            db.SaveChanges();
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
