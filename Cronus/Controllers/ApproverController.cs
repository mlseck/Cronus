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
using Cronus.Login;

namespace Cronus.Controllers
{
    public class ApproverController : Controller
    {
        private CronusDatabaseEntities db = new CronusDatabaseEntities();

        // GET: Approver
        public ActionResult Index()
        {
            var loggedinEmp = db.employees.Find(UserManager.User.employeeID);

            //loggedinEmp.employeeGroupManaged

            ViewBag.GroupId = loggedinEmp.employeeGroupManaged;
            return View(db.timeperiods.ToList());
        }

        public ActionResult ApproverIndex(int groupID, DateTime periodEnddate)
        {
            var perioudEndDateQuery = db.timeperiods.Find(periodEnddate);

            var groupQuery = db.groups.Find(groupID);

            int[] projectIds = groupQuery.projects.Select(x => x.projectID).ToArray();
            var projectsQuery = (from p in db.projects where (projectIds.Contains(p.projectID)) select p).ToList();

            string [] employeeIds = groupQuery.employees.Select(x => x.employeeID).ToArray();
            var employeeQuery = (from e in db.employees where (employeeIds.Contains(e.employeeID)) select e).ToList();

            // var timeperiods = db.timeperiods.ToArray();
            var employeeTimeperiods = (from et in db.employeetimeperiods where (et.TimePeriod_periodEndDate.Equals(periodEnddate) && employeeIds.Contains(et.Employee_employeeID)) select et).ToList();
            //var employeeTimeperiods = (from et in db.employeetimeperiods where (periodEnddate.Equals(et.TimePeriod_periodEndDate) && perioudEndDateQuery.employeetimeperiods.Select(e => e.Employee_employeeID).Contains(et.timeperiod)) select et).ToList();

            //getting the first timeperiod to test functionality. Dynamicism will be implemented later
            //timeperiod timeperiod = db.timeperiods.FirstOrDefault();

            ApproverViewModel approverViewModel = new ApproverViewModel();
            approverViewModel.Projects = projectsQuery;
            approverViewModel.Activities = db.activities.ToList();
            approverViewModel.HoursWorkedList = db.hoursworkeds.ToList();
            approverViewModel.Employees = employeeQuery;
            approverViewModel.timeperiod = perioudEndDateQuery;
            approverViewModel.isApproved = false;
            approverViewModel.employeetimeperiods = employeeTimeperiods.ToList();
            //approverViewModel.ActivityNames = new SelectList(myModel.Activities, "activityID", "activityName");

            return View(approverViewModel);
        }

        [HttpPost]
        public ActionResult ApproverIndex(ApproverViewModel approverViewModel)
        {
            if (ModelState.IsValid)
            {

                //if (project.activityIds != null)
                //{
                //    project.activities = (from s in this.activityRepository.All where project.activityIds.Contains(s.activityID) select s).ToList();
                //}
                foreach(employeetimeperiod employeetp in approverViewModel.employeetimeperiods)
                {
                    db.Entry(employeetp).State = EntityState.Modified;
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
