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
using Cronus.ViewModels;

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
            //GroupViewModel groupViewModel = new GroupViewModel();
            //groupViewModel.Groups = groupRepository.AllIncluding(group => group.projects, group => group.employees);
            //groupViewModel

            ViewBag.PossibleProjects = projectRepository.All;
            ViewBag.PossibleEmployees = employeeRepository.All;

            return View(groupRepository.AllIncluding(group => group.projects, group => group.employees));
        }

        //
        // GET: /Group/Details/5

        public ViewResult Details(int id)
        {
            return View(groupRepository.FindGroup(id));
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
            var managers = groupRepository.All.Select(g => g.groupManager).ToList();

            List<employee> availableManagerList = employeeRepository.All.Select(e => e).Where(e => !managers.Contains(e.employeeID)).ToList();


            model.empList = availableManagerList.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.employeeLastName.ToString(),
                    Value = a.employeeID.ToString()
                };
            });


            ViewBag.PossibleProjects = projectRepository.All;

            ViewBag.PossibleEmployees = employeeRepository.All.ToList();

            return View(model);
        }


        public ActionResult ProjectList(int GroupID)
        {
            ViewBag.GroupId = GroupID;

            var group = groupRepository.FindGroup(GroupID);

            ViewBag.GroupName = group.groupName;

            //if (group == null)
            //{
            //    var newProjects = new List<project>();
            //    //{
            //    //    projectIds = new int[0]
            //    //};

            //    return View("ProjectIndex", newProjects.ToList());
            //}
            //ViewBag.ProjectID = id;
            //var addresses = db.Addresses.Where(a => a.PersonID == id).OrderBy(a => a.City);

            //(from s in this.activityRepository.All where project.activityIds.Contains(s.activityID) select s).ToList();
            //int[] projectIds = group.projects.Select(x => x.projectID).ToArray();

            //var projects = from s in this.projectRepository.All where projectIds.Contains(s.projectID) select s;

            return View("ProjectIndex", group);
        }

        public ActionResult EmployeeList(int GroupID)
        {
            ViewBag.GroupId = GroupID;

            var group = groupRepository.FindGroup(GroupID);

            ViewBag.GroupName = group.groupName;


            //if (group == null)
            //{
            //    var newEmployees = new List<employee>();

            //    return PartialView("EmployeeIndex", newEmployees.ToList());
            //}
            //string[] employeesIds = group.employees.Select(x => x.employeeID).ToArray();

            //var employeess = from s in this.employeeRepository.All where employeesIds.Contains(s.employeeID) select s;

            return View("EmployeeIndex", group);

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

        //
        // GET: /Group/Edit/5

        public ActionResult Edit(int GroupID)
        {
            ViewBag.PossibleProjects = projectRepository.All;

            ViewBag.PossibleEmployees = employeeRepository.All;

            group model = groupRepository.FindGroup(GroupID);

            employee manager = employeeRepository.Find(model.groupManager);

            var managers = groupRepository.All.Select(g => g.groupManager).ToList();

            List<employee> availableManagerList = employeeRepository.All.Select(e => e).Where(e => !managers.Contains(e.employeeID)).ToList();

            availableManagerList.Add(manager);

            model.empList = availableManagerList.ConvertAll(a =>
            {
                return new SelectListItem()
                {
                    Text = a.employeeLastName.ToString(),
                    Value = a.employeeID.ToString(),
                    
                };
            });

            model.empList.First(x => x.Value == model.groupManager).Selected = true;

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
                group originalGroup = this.groupRepository.FindGroup(group.groupID);

                originalGroup.groupName = group.groupName;
                originalGroup.groupManager = group.groupManager;
                
                //if (project.activityIds != null)
                //{
                //    originalProject.activities = (from s in this.activityRepository.All where project.activityIds.Contains(s.activityID) select s).ToList();
                //}

                groupRepository.InsertOrUpdate(originalGroup);
                groupRepository.Save();
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
            return View(groupRepository.FindGroup(id));
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
