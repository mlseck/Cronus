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

        public ViewResult Index()
        {
            return View(activityRepository.AllIncluding(activity => activity.projects));
        }

        //
        // GET: /Activity/Details/5

        public ViewResult Details(int id)
        {
            return View(activityRepository.Find(id));
        }

        //
        // GET: /Activity/Create

        public ActionResult Create()
        {
            activity model = new activity
            {
                projectIds = new int[0]
            };

            ViewBag.PossibleProjects = projectRepository.All;

            return View(model);
        }

        //
        // POST: /Activity/Create

        [HttpPost]
        public ActionResult Create(activity activity)
        {
            if (ModelState.IsValid)
            {

                if (activity.projectIds != null)
                {
                    activity.projects = (from s in this.projectRepository.All where activity.projectIds.Contains(s.projectID) select s).ToList();
                }

                activityRepository.InsertOrUpdate(activity);
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
