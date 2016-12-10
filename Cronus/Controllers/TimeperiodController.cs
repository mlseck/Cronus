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
    public class TimeperiodController : Controller
    {
        private CronusDatabaseEntities db = new CronusDatabaseEntities();

        // GET: Timeperiod
        public ActionResult Index()
        {
            return View(db.timeperiods.ToList());
        }

        // GET: Timeperiod/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            timeperiod timeperiod = db.timeperiods.Find(id);
            if (timeperiod == null)
            {
                return HttpNotFound();
            }
            return View(timeperiod);
        }

        // GET: Timeperiod/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Timeperiod/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "periodEndDate")] timeperiod timeperiod)
        {
            if (ModelState.IsValid)
            {
                db.timeperiods.Add(timeperiod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(timeperiod);
        }

        // GET: Timeperiod/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            timeperiod timeperiod = db.timeperiods.Find(id);
            if (timeperiod == null)
            {
                return HttpNotFound();
            }
            return View(timeperiod);
        }

        // POST: Timeperiod/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "periodEndDate")] timeperiod timeperiod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeperiod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timeperiod);
        }

        // GET: Timeperiod/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            timeperiod timeperiod = db.timeperiods.Find(id);
            if (timeperiod == null)
            {
                return HttpNotFound();
            }
            return View(timeperiod);
        }

        // POST: Timeperiod/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            timeperiod timeperiod = db.timeperiods.Find(id);
            db.timeperiods.Remove(timeperiod);
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
