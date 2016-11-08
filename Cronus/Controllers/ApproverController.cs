using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseEntities;
using Cronus.ViewModels;


namespace Cronus.Controllers
{
    public class ApproverController : Controller
    {
        private CronusDatabaseEntities db = new CronusDatabaseEntities();

        // GET: Approver
        public ActionResult Index()
        {
            ViewBag.GroupId = 1;
            return View(db.timeperiods.ToList());
        }

        public ActionResult ApproverIndex(int groupID)
        {

            var groupQuery = db.groups.Find(groupID);

            int[] projectIds = groupQuery.projects.Select(x => x.projectID).ToArray();
            var projectsQuery = (from p in db.projects where (projectIds.Contains(p.projectID)) select p).ToList();

            string [] employeeIds = groupQuery.employees.Select(x => x.employeeID).ToArray();
            var employeeQuery = (from e in db.employees where (employeeIds.Contains(e.employeeID)) select e).ToList();

            //getting the first timeperiod to test functionality. Dynamicism will be implemented later
            timeperiod timeperiod = db.timeperiods.FirstOrDefault();

            ApproverViewModel approverViewModel = new ApproverViewModel();
            approverViewModel.Projects = projectsQuery;
            approverViewModel.Activities = db.activities.ToList();
            approverViewModel.HoursWorkedList = db.hoursworkeds.ToList();
            approverViewModel.Employees = employeeQuery;
            approverViewModel.timeperiod = timeperiod;
            //approverViewModel.ActivityNames = new SelectList(myModel.Activities, "activityID", "activityName");

            return View(approverViewModel);
        }
        //// GET: Approver/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    project project = db.projects.Find(id);
        //    if (project == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(project);
        //}

        //// GET: Approver/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Approver/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "projectID,projectName,projectStartDate,projectEndDate,projectDescription,projectCapitalCode,projectAbbreviation,projectActive")] project project)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.projects.Add(project);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(project);
        //}

        //// GET: Approver/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    project project = db.projects.Find(id);
        //    if (project == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(project);
        //}

        //// POST: Approver/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "projectID,projectName,projectStartDate,projectEndDate,projectDescription,projectCapitalCode,projectAbbreviation,projectActive")] project project)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(project).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(project);
        //}

        //// GET: Approver/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    project project = db.projects.Find(id);
        //    if (project == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(project);
        //}

        //// POST: Approver/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    project project = db.projects.Find(id);
        //    db.projects.Remove(project);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
