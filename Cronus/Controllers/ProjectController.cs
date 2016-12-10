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
    public class ProjectController : Controller
    {
        private readonly IProjectRepository projectRepository;
        private readonly IActivityRepository activityRepository;
        private readonly IGroupRepository groupRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public ProjectController() : this(new ProjectRepository(), new ActivityRepository(), new GroupRepository())
        {
        }

        public ProjectController(IProjectRepository projectRepository, IActivityRepository activityRepository, IGroupRepository groupRepository)
        {
            this.projectRepository = projectRepository;
            this.activityRepository = activityRepository;
            this.groupRepository = groupRepository;
        }

        //
        // GET: /Project/

        public ViewResult Index(string searchString)
        {

            var projectList = projectRepository.AllIncluding(project => project.activities);

            if (!String.IsNullOrEmpty(searchString))
            {
                projectList = projectList.Where(s => s.projectName.Contains(searchString));
            }
            return View(projectList);
        }
        public ActionResult ProjectIndex(int id)
        {
            ViewBag.GroupId = id;

            var group = groupRepository.FindGroup(id);

            if (group == null)
            {
                var newProjects = new List<project>();
                //{
                //    projectIds = new int[0]
                //};

                return PartialView("_index", newProjects.ToList());
            }
            //ViewBag.ProjectID = id;
            //var addresses = db.Addresses.Where(a => a.PersonID == id).OrderBy(a => a.City);

            //(from s in this.activityRepository.All where project.activityIds.Contains(s.activityID) select s).ToList();
            int[] projectIds = group.projects.Select(x => x.projectID).ToArray();

            var projects = from s in this.projectRepository.All where projectIds.Contains(s.projectID) select s;

            return PartialView("_index", projects.ToList());
        }

        
         //GET: /Project/Details/5

        public ViewResult Details(int id)
        {
            return View(projectRepository.Find(id));
        }

        public ViewResult ProjectActivities(int id)
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

        public ActionResult AddProjects(int GroupId)
        {
            //iewBag.ProjectID = ProjectID;

            group model = groupRepository.FindGroup(GroupId);

            ViewBag.PossibleProjects = projectRepository.All;

            //model.selectedProject = ProjectID;

            //var project = projectRepository.Find(ProjectID);
            //model.projects.Add(project);
            //{
            //    projectIds = new int[0]
            //};


            return PartialView("ProjectCheckList", model);
        }

        [HttpPost]
        public ActionResult AddProjects(group group)
        {
            if (ModelState.IsValid)
            {
                group originalGroup = this.groupRepository.FindGroup(group.groupID);

                originalGroup.groupName = group.groupName;

                originalGroup.projects.Clear();


                if (group.projectIds != null)
                {
                    originalGroup.projects = (from s in this.projectRepository.All where @group.projectIds.Contains(s.projectID) select s).ToList();
                }

                groupRepository.InsertOrUpdate(originalGroup, null);

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    groupRepository.Save();
                }
                catch (DbEntityValidationException e)
                {

                }

                //projectRepository.Save();
                string url = Url.Action("ProjectIndex", "Project", new { id = originalGroup.groupID });
                return Json(new { success = true, url = url });
            }
            else
            {
                ViewBag.PossibleProjects = projectRepository.All;
                return PartialView("AddProjects", group);
            }
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

            try
            {
                model.activityIds = (from s in model.activities select s.activityID).ToArray();
            }
            catch(Exception e)
            {
                throw e;
            }
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
                originalProject.projectStartDate = project.projectStartDate;
                originalProject.projectEndDate = project.projectEndDate;
                originalProject.projectDescription = project.projectDescription;
                originalProject.projectCapitalCode = project.projectCapitalCode;
                originalProject.projectAbbreviation = project.projectAbbreviation;
                originalProject.projectActive = project.projectActive;
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
