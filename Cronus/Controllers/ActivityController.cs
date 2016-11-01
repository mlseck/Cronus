using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseEntities;
using Cronus.Models;
using System.Data.Entity.Validation;

namespace Cronus.Controllers
{
    public class ActivityController : Controller
    { 
        private readonly IActivityRepository activityRepository;
        private readonly IProjectRepository projectRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public ActivityController() : this(new ActivityRepository(), new ProjectRepository())
        {
        }

        public ActivityController(IActivityRepository activityRepository, IProjectRepository projectRepository)
        {
            this.activityRepository = activityRepository;
            this.projectRepository = projectRepository;
        }

        //
        // GET: /Activity/

        //public ActionResult Index()
        //{
        //    return PartialView("Index", activityRepository.AllIncluding(activity => activity.projects));

        //}

        public ActionResult Index(int id)
        {
            ViewBag.ProjectID = id;

            var project = projectRepository.Find(id);

            if (project == null)
            {
                var newActivities = new List<activity>();
                //{
                //    projectIds = new int[0]
                //};

                return PartialView("Index", newActivities.ToList());
            }
            //ViewBag.ProjectID = id;
            //var addresses = db.Addresses.Where(a => a.PersonID == id).OrderBy(a => a.City);

            //(from s in this.activityRepository.All where project.activityIds.Contains(s.activityID) select s).ToList();
            int[] activityIds = project.activities.Select(x => x.activityID).ToArray();

            var activities = from s in this.activityRepository.All where activityIds.Contains(s.activityID) select s;

            return PartialView("Index", activities.ToList());
        }

        //
        // GET: /Activity/Details/5

        //public ViewResult Details(int id)
        //{
        //    return View(activityRepository.Find(id));
        //}

        //
        // GET: /Activity/Create

        public ActionResult Create(int ProjectID)
        {
            activity model = new activity();

            model.selectedProject = ProjectID;

            //var project = projectRepository.Find(ProjectID);
            //model.projects.Add(project);
            //{
            //    projectIds = new int[0]
            //};

            ViewBag.PossibleProjects = projectRepository.All;

            return PartialView("Create", model);
        }

        // GET: /Activity/List

        public ActionResult AddActivities(int ProjectID)
        {
            ViewBag.ProjectID = ProjectID;

            project model = projectRepository.Find(ProjectID);

            ViewBag.PossibleActivities = activityRepository.All;

            //model.selectedProject = ProjectID;

            //var project = projectRepository.Find(ProjectID);
            //model.projects.Add(project);
            //{
            //    projectIds = new int[0]
            //};


            return PartialView("List", model);
        }

        [HttpPost]
        public ActionResult AddActivities(project project)
        {
            if (ModelState.IsValid)
            {
                project originalProject = this.projectRepository.Find(project.projectID);

                originalProject.projectName = project.projectName;
                originalProject.projectStartDate = project.projectStartDate;
                originalProject.projectEndDate = project.projectEndDate;
                originalProject.projectDescription = project.projectDescription;
                originalProject.projectCapitalCode = project.projectCapitalCode;
                originalProject.projectAbbreviation = project.projectAbbreviation;
                originalProject.projectActive = project.projectActive;

                originalProject.activities.Clear();


                if (project.activityIds != null)
                {
                    originalProject.activities = (from s in this.activityRepository.All where project.activityIds.Contains(s.activityID) select s).ToList();
                }

                projectRepository.InsertOrUpdate(originalProject);


                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    projectRepository.Save();
                }
                catch (DbEntityValidationException e)
                {
                    
                }

                //projectRepository.Save();




                string url = Url.Action("Index", "Activity", new { id = originalProject.projectID });
                return Json(new { success = true, url = url });
            }
            else
            {
                ViewBag.PossibleActivities = activityRepository.All;
                return PartialView("AddActivities", project);
            }
        }

        //
        // POST: /Activity/Create

        [HttpPost]
        public ActionResult Create(activity activity)
        {
            if (ModelState.IsValid)
            {

                //activity.projects = (from s in this.projectRepository.All where activity.projectIds.Contains(s.projectID) select s).ToList();

                var project = projectRepository.Find(activity.selectedProject);
                activity.projects.Add(project);

                activityRepository.InsertOrUpdate(activity);
                activityRepository.Save();
                //                return RedirectToAction("Index");
                string url = Url.Action("Index", "Activity", new { id = activity.selectedProject });
                return Json(new { success = true, url = url });

            }
            else
            {
                ViewBag.PossibleProjects = projectRepository.All;
                return PartialView("Create",activity);
            }
        }

        //
        // GET: /Activity/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBag.PossibleProjects = projectRepository.All;

            activity model = activityRepository.Find(id);

            model.projectIds = (from s in model.projects select s.projectID).ToArray();

            return View(model);
        }

        //
        // POST: /Activity/Edit/5

        [HttpPost]
        public ActionResult Edit(activity activity)
        {
            if (ModelState.IsValid)
            {
                activity originalActivity = this.activityRepository.Find(activity.activityID);

                originalActivity.activityName = activity.activityName;

                originalActivity.projects.Clear();

                if (activity.projectIds != null)
                {
                    originalActivity.projects = (from s in this.projectRepository.All where activity.projectIds.Contains(s.projectID) select s).ToList();
                }

                activityRepository.InsertOrUpdate(originalActivity);
                activityRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleProjects = projectRepository.All;
                return View(activity);
            }

        }

        //
        // GET: /Activity/Delete/5

        public ActionResult Delete(int id)
        {
            return View(activityRepository.Find(id));
        }

        //
        // POST: /Activity/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            activityRepository.Delete(id);
            activityRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                activityRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
