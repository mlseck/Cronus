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
    public class ProjectController : Controller
    {
        private readonly IProjectRepository projectRepository;
        private readonly IActivityRepository activityRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public ProjectController() : this(new ProjectRepository(), new ActivityRepository())
        {
        }

        public ProjectController(IProjectRepository projectRepository, IActivityRepository activityRepository)
        {
            this.projectRepository = projectRepository;
            this.activityRepository = activityRepository;
        }

        //
        // GET: /Project/

        public ViewResult Index()
        {
            return View(projectRepository.AllIncluding(project => project.activities));
        }

        //
        // GET: /Project/Details/5

        public ViewResult Details(int id)
        {
            return View(projectRepository.Find(id));
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            project model = new project
            {
                activityIds = new int[0]
            };

            ViewBag.PossibleActivities = activityRepository.All;

            return View(model);
        }

        //
        // POST: /Project/Create

        [HttpPost]
        public ActionResult Create(project project)
        {
            if (ModelState.IsValid)
            {

                if (project.activityIds != null)
                {
                    project.activities = (from s in this.activityRepository.All where project.activityIds.Contains(s.activityID) select s).ToList();
                }

                projectRepository.InsertOrUpdate(project);
                projectRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleActivities = activityRepository.All;
                return View(project);
            }
        }

        //
        // GET: /Project/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBag.PossibleActivities = activityRepository.All;

            project model = projectRepository.Find(id);

            model.activityIds = (from s in model.activities select s.activityID).ToArray();

            return View(model);
        }

        //
        // POST: /Project/Edit/5

        [HttpPost]
        public ActionResult Edit(project project)
        {
            if (ModelState.IsValid)
            {
                project originalProject = this.projectRepository.Find(project.projectID);

                originalProject.projectName = project.projectName;

                originalProject.activities.Clear();

                if (project.activityIds != null)
                {
                    originalProject.activities = (from s in this.activityRepository.All where project.activityIds.Contains(s.activityID) select s).ToList();
                }

                projectRepository.InsertOrUpdate(originalProject);
                projectRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleActivities = activityRepository.All;
                return View(project);
            }

        }

        //
        // GET: /Project/Delete/5

        public ActionResult Delete(int id)
        {
            return View(projectRepository.Find(id));
        }

        //
        // POST: /Project/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            projectRepository.Delete(id);
            projectRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                projectRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
