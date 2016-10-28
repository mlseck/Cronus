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
    public class FavoriteController : Controller
    {
        private CronusDatabaseEntities db = new CronusDatabaseEntities();

        // GET: Favorite
        public ActionResult Index()
        {
            var favorites = db.favorites.Include(f => f.activity).Include(f => f.employee);
            return View(favorites.ToList());
        }

        // GET: Favorite/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            favorite favorite = db.favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            return View(favorite);
        }

        // GET: Favorite/Create
        public ActionResult Create()
        {
            ViewBag.Activity_activityID = new SelectList(db.activities, "activityID", "activityName");
            ViewBag.Employee_employeeID = new SelectList(db.employees, "employeeID", "employeeFirstName");
            return View();
        }

        // POST: Favorite/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Employee_employeeID,Activity_activityID,favoriteID")] favorite favorite)
        {
            if (ModelState.IsValid)
            {
                db.favorites.Add(favorite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Activity_activityID = new SelectList(db.activities, "activityID", "activityName", favorite.Activity_activityID);
            ViewBag.Employee_employeeID = new SelectList(db.employees, "employeeID", "employeeFirstName", favorite.Employee_employeeID);
            return View(favorite);
        }

        // GET: Favorite/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            favorite favorite = db.favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            ViewBag.Activity_activityID = new SelectList(db.activities, "activityID", "activityName", favorite.Activity_activityID);
            ViewBag.Employee_employeeID = new SelectList(db.employees, "employeeID", "employeeFirstName", favorite.Employee_employeeID);
            return View(favorite);
        }

        // POST: Favorite/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Employee_employeeID,Activity_activityID,favoriteID")] favorite favorite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favorite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Activity_activityID = new SelectList(db.activities, "activityID", "activityName", favorite.Activity_activityID);
            ViewBag.Employee_employeeID = new SelectList(db.employees, "employeeID", "employeeFirstName", favorite.Employee_employeeID);
            return View(favorite);
        }

        // GET: Favorite/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            favorite favorite = db.favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            return View(favorite);
        }

        // POST: Favorite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            favorite favorite = db.favorites.Find(id);
            db.favorites.Remove(favorite);
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
