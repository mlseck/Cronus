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
    public class groupsController : Controller
    {
        private readonly IProjectRepository projectRepository;
        private readonly IemployeeRepository employeeRepository;
        private readonly IGroupRepository groupRepository;


        // If you are using Dependency Injection, you can delete the following constructor
        public groupsController() : this(new GroupRepository(), new ProjectRepository(), new EmployeeRepository())
        {
        }

        public groupsController(IGroupRepository groupRepository, IProjectRepository projectRepository, IemployeeRepository employeeRepository)
        {
            this.groupRepository = groupRepository;
            this.projectRepository = projectRepository;
            this.employeeRepository = employeeRepository;


        }

        //
        // GET: /Group/

        public ViewResult Index()
        {
            return View(groupRepository.AllIncluding(group => group.projects, group => group.employees));
        }

        //
        // GET: /Group/Details/5

        public ViewResult Details(int id)
        {
            return View(groupRepository.Find(id));
        }



        //
        // GET: /Group/Create

        public ActionResult Create()
        {
            group model = new group
            {
                projectIds = new int[0],
                employeeIds = new string[0]
            };

            

            ViewBag.PossibleProjects = projectRepository.All;

            ViewBag.PossibleEmployees = employeeRepository.All;

            return View(model);
        }


        public ActionResult ProjectList(int id)
        {
            ViewBag.GroupId = id;

            var group = groupRepository.Find(id);

            if (group == null)
            {
                var newProjects = new List<project>();
                //{
                //    projectIds = new int[0]
                //};

                return PartialView("ProjectIndex", newProjects.ToList());
            }
            //ViewBag.ProjectID = id;
            //var addresses = db.Addresses.Where(a => a.PersonID == id).OrderBy(a => a.City);

            //(from s in this.activityRepository.All where project.activityIds.Contains(s.activityID) select s).ToList();
            int[] projectIds = group.projects.Select(x => x.projectID).ToArray();

            var projects = from s in this.projectRepository.All where projectIds.Contains(s.projectID) select s;

            return PartialView("ProjectIndex", projects.ToList());
        }

        public ActionResult EmployeeList(int id)
        {
            ViewBag.GroupId = id;

            var group = groupRepository.Find(id);

            if (group == null)
            {
                var newEmployees = new List<employee>();

                return PartialView("EmployeeIndex", newEmployees.ToList());
            }
            string[] employeesIds = group.employees.Select(x => x.employeeID).ToArray();

            var employeess = from s in this.employeeRepository.All where employeesIds.Contains(s.employeeID) select s;

            return PartialView("EmployeeIndex", employeess.ToList());

        }

        //
        // POST: /Group/Create

        [HttpPost]
        public ActionResult Create(group group)
        {
            if (ModelState.IsValid)
            {

                if (group.projectIds != null)
                {
                    group.projects = (from s in this.projectRepository.All where @group.projectIds.Contains(s.projectID) select s).ToList();
                }

                if (group.employeeIds != null)
                {
                    group.employees = (from s in this.employeeRepository.All where @group.employeeIds.Contains(s.employeeID) select s).ToList();
                }

                groupRepository.InsertOrUpdate(group);
                groupRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleProjects = projectRepository.All;

                ViewBag.PossibleEmployees = employeeRepository.All;

                return View(group);
            }
        }

        public ActionResult AddProjects(int GroupId)
        {
            //iewBag.ProjectID = ProjectID;

            group model = groupRepository.Find(GroupId);

            ViewBag.PossibleProjects = projectRepository.All;

            //model.selectedProject = ProjectID;

            //var project = projectRepository.Find(ProjectID);
            //model.projects.Add(project);
            //{
            //    projectIds = new int[0]
            //};


            return PartialView("ProjectCheckList", model);
        }

        public ActionResult AddEmployees(int GroupId)
        {
            //iewBag.ProjectID = ProjectID;

            group model = groupRepository.Find(GroupId);

            ViewBag.PossibleEmployees = employeeRepository.All;

            return PartialView("EmployeeCheckList", model);
        }



        [HttpPost]
        public ActionResult AddProjects(group group)
        {
            if (ModelState.IsValid)
            {
                group originalGroup = this.groupRepository.Find(group.groupID);

                originalGroup.groupName = group.groupName;

                originalGroup.projects.Clear();


                if (group.projectIds != null)
                {
                    originalGroup.projects = (from s in this.projectRepository.All where @group.projectIds.Contains(s.projectID) select s).ToList();
                }

                groupRepository.InsertOrUpdate(originalGroup);


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




                string url = Url.Action("_index", "Project", new { id = originalGroup.groupID});
                return Json(new { success = true, url = url });
            }
            else
            {
                ViewBag.PossibleProjects = projectRepository.All;
                return PartialView("AddProjects", group);
            }
        }



        [HttpPost]
        public ActionResult AddEmployees(group group)
        {
            if (ModelState.IsValid)
            {
                group originalGroup = this.groupRepository.Find(group.groupID);

                originalGroup.groupName = group.groupName;

                originalGroup.employees.Clear();


                if (group.employeeIds != null)
                {
                    originalGroup.employees = (from s in this.employeeRepository.All where @group.employeeIds.Contains(s.employeeID) select s).ToList();
                }

                groupRepository.InsertOrUpdate(originalGroup);


                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    groupRepository.Save();
                }
                catch (DbEntityValidationException e)
                {

                }

                //EmployeeRepository.Save();




                string url = Url.Action("_index", "Employee", new { id = originalGroup.groupID });
                return Json(new { success = true, url = url });
            }
            else
            {
                ViewBag.PossibleEmployees = employeeRepository.All;
                return PartialView("AddEmployees", group);
            }
        }
        //
        // GET: /Group/Edit/5

        public ActionResult Edit(int id)
        {
            ViewBag.PossibleProjects = projectRepository.All;

            ViewBag.PossibleEmployees = employeeRepository.All;

            group model = groupRepository.Find(id);

            model.projectIds = (from s in model.projects select s.projectID).ToArray();

            model.employeeIds = (from s in model.employees select s.employeeID).ToArray();

            return View(model);
        }

        //
        // POST: /Group/Edit/5

        [HttpPost]
        public ActionResult Edit(group group)
        {
            if (ModelState.IsValid)
            {
                group originalProject = this.groupRepository.Find(group.groupID);

                //originalProject.projectName = project.projectName;
                //originalProject.projectStartDate = project.projectStartDate;
                //originalProject.projectEndDate = project.projectEndDate;
                //originalProject.projectDescription = project.projectDescription;
                //originalProject.projectCapitalCode = project.projectCapitalCode;
                //originalProject.projectAbbreviation = project.projectAbbreviation;
                //originalProject.projectActive = project.projectActive;
                //originalProject.projectName = project.projectName;


                //originalProject.activities.Clear();

                //if (project.activityIds != null)
                //{
                //    originalProject.activities = (from s in this.activityRepository.All where project.activityIds.Contains(s.activityID) select s).ToList();
                //}

                //projectRepository.InsertOrUpdate(originalProject);
                //projectRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                //ViewBag.PossibleActivities = activityRepository.All;
                return View(group);
            }

        }

        //
        // GET: /Project/Delete/5

        public ActionResult Delete(int id)
        {
            return View(groupRepository.Find(id));
        }

        //
        // POST: /Project/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            groupRepository.Delete(id);
            groupRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                groupRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
