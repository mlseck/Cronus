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
    public class EmployeeController : Controller
    {
        private readonly IFavoriteRepository favoriteRepository;
        private readonly IemployeeRepository employeeRepository;

        // If you are using Dependency Injection, you can delete the following constructor
        public EmployeeController() : this(new EmployeeRepository(), new FavoriteRepository())
        {
        }

        public EmployeeController(IemployeeRepository employeeRepository, IFavoriteRepository favoriteRepository)
        {
            this.employeeRepository = employeeRepository;
            this.favoriteRepository = favoriteRepository;
        }

        //
        // GET: /employee/

        public ViewResult Index()
        {
            return View(employeeRepository.AllIncluding(employee => employee.favorites));
        }

        //
        // GET: /employee/Details/5

        public ViewResult Details(string id)
        {
            return View(employeeRepository.Find(id));
        }

        //
        // GET: /employee/Create

        public ActionResult Create()
        {
            employee model = new employee
            {
                favoriteIds = new int[0]
            };

            ViewBag.PossibleFavorites = favoriteRepository.All;

            return View(model);
        }

        //
        // POST: /employee/Create

        [HttpPost]
        public ActionResult Create(employee employee)
        {
            if (ModelState.IsValid)
            {

                if (employee.favoriteIds != null)
                {
                    employee.favorites = (from s in this.favoriteRepository.All where employee.favoriteIds.Contains(s.favoriteID) select s).ToList();
                }

                employeeRepository.InsertOrUpdate(employee);
                employeeRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleActivities = favoriteRepository.All;
                return View(employee);
            }
        }

        //
        // GET: /employee/Edit/5

        public ActionResult Edit(string id)
        {
            ViewBag.PossibleActivities = favoriteRepository.All;

            employee model = employeeRepository.Find(id);

            model.favoriteIds = (from s in model.favorites select s.favoriteID).ToArray();

            return View(model);
        }

        //
        // POST: /employee/Edit/5

        [HttpPost]
        public ActionResult Edit(employee employee)
        {
            if (ModelState.IsValid)
            {
                employee originalemployee = this.employeeRepository.Find(employee.employeeID);

                originalemployee.employeeLastName = employee.employeeLastName;

                originalemployee.favorites.Clear();

                if (employee.favoriteIds != null)
                {
                    originalemployee.favorites = (from s in this.favoriteRepository.All where employee.favoriteIds.Contains(s.favoriteID) select s).ToList();
                }

                employeeRepository.InsertOrUpdate(originalemployee);
                employeeRepository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.PossibleActivities = favoriteRepository.All;
                return View(employee);
            }

        }

        //
        // GET: /employee/Delete/5

        public ActionResult Delete(string id)
        {
            return View(employeeRepository.Find(id));
        }

        //
        // POST: /employee/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            employeeRepository.Delete(id);
            employeeRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                employeeRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
