using DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cronus.Models;
using Cronus.ViewModels;

namespace Cronus.Controllers
{
    public class HomeController : Controller
    {
        private CronusDatabaseEntities db = new CronusDatabaseEntities();
        HomeViewModel myModel = new HomeViewModel();


        public ActionResult Index()
        {
            HomeViewModel myModel = new HomeViewModel();
            myModel.Projects = db.projects.ToList();
            myModel.Activities = db.activities.ToList();
            myModel.ProjectList = new SelectList(myModel.Projects, "projectID", "projectName");
            myModel.ActivityList = new SelectList(myModel.Activities, "activityID", "activityName");
            myModel.HoursWorked = db.hoursworkeds.ToList();
            return View(myModel);

        }

        //Method will update the activities dropdownlist in index page when specific project is picked
        [HttpPost]
        [ActionName("UpdateActivities")]
        public ActionResult UpdateActivities(HomeViewModel myModel)
        {
            myModel.Projects = db.projects.ToList();
            myModel.Activities = db.activities.ToList();
            myModel.ProjectList = new SelectList(myModel.Projects, "projectID", "projectName");
            project Selected = db.projects.Find(myModel.SelectedProjectID);
            myModel.ActivityList = new SelectList(Selected.activities.ToList(), "activityID", "activityName");
            return View("Index", myModel);
        }

        public ActionResult Monthly()
        {
            MonthlyViewModel monthlyModel = new MonthlyViewModel();
            return View(monthlyModel);
        }

        public ActionResult GetEvents()
        {
            //will get projects/activities for the month.
            MonthlyViewModel monthlyModel = new MonthlyViewModel();

            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);


            monthlyModel.Projects = db.projects
                                            .Where(n => n.projectEndDate >= startDate)
                                            .Where(n => n.projectEndDate <= endDate);

            return Json(monthlyModel, JsonRequestBehavior.AllowGet);




        }

        [HttpGet]
        public JsonResult GetBetweenDates(string employeeID)
        {
            HomeViewModel homeModel = new HomeViewModel();

            //DateTime previousWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 7);
            DateTime startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
            DateTime endDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);


            //when getting previous week, must have the start of the time period be the previous monday.
            //For example lets say its sunday and you click previous week, youll be getting the incorrect dates.

            //if 1 week ago was tuesday, set start to 1 day previous, so start date startes on a monday, ends on a sunday.
            if (startDate.DayOfWeek == DayOfWeek.Tuesday)
            {
                startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            }
            //if 1 week ago was wednesday, set start to 2 day previous, so start date startes on a monday, ends on a sunday.
            //repeat for each day.
            else if (startDate.DayOfWeek == DayOfWeek.Wednesday)
            {
                startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 1);
            }
            else if (startDate.DayOfWeek == DayOfWeek.Thursday)
            {
                startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 2);
            }
            else if (startDate.DayOfWeek == DayOfWeek.Friday)
            {
                startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 3);
            }
            else if (startDate.DayOfWeek == DayOfWeek.Saturday)
            {
                startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 4);
            }
            else if (startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                startDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 5);
            }


            homeModel.HoursWorked = db.hoursworkeds
                                                .Where(n => n.date >= startDate)
                                                .Where(n => n.date <= endDate)
                                                .Where(n => n.TimePeriod_Employee_employeeID == employeeID);

            return Json(homeModel, JsonRequestBehavior.AllowGet);
        }


        //going to be working on saving the hours listed into the DB.
        [HttpPost]
        public ActionResult SaveHours(HomeViewModel model)
        {
            //pass through a model filled with information, push that information to the corresponding DB table.
            hoursworked hoursWorked = new hoursworked();
            hoursWorked.comments = model.hrsWorked.comments;
            hoursWorked.date = model.hrsWorked.date;
            //finish that later
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Return Favorite View with All activities and User's Favorites
        [HttpGet]
        public ActionResult Favorite()
        {
            ViewBag.Message = "Your Favorites Page";
            FavoriteViewModel myModel = new FavoriteViewModel();
            myModel.Activities = db.activities.ToList();
            myModel.Favorites = db.favorites.ToList();
            myModel.ActivityNames = new SelectList(myModel.Activities, "activityID", "activityName");
            var query = from a in db.activities
                        join b in db.favorites
                        on a.activityID equals b.Activity_activityID
                        where b.Employee_employeeID.Equals("5X67H8")
                        select new FavoriteViewModel { SelectedActivity = a, SelectedFavorite = b };
            myModel.UserFavorites = new SelectList(query.ToArray(), "SelectedFavorite.favoriteID", "SelectedActivity.ActivityName");
            return View(myModel);
        }

        // Add Selected Activity to Favorite Table
        [HttpPost]
        public ActionResult AddFavorite(FavoriteViewModel favorite)
        {
            //Need to make sure it's not adding favorites that already exist
            ViewBag.Message = "Your Favorites Page";
            int selectedActivity = favorite.selectedActivityID;
            if (selectedActivity != 0)
            {
                favorite AddFavorite = new favorite();
                AddFavorite.Activity_activityID = selectedActivity;
                AddFavorite.Employee_employeeID = "5X67H8";
                // Check if selected activity is already a favorite. If not, add to Favorite Table
                var existsQuery = from f in db.favorites
                                  where (f.Activity_activityID.Equals(AddFavorite.Activity_activityID) && f.Employee_employeeID.Equals("5X67H8"))
                                  select f;
                if (!existsQuery.Any())
                {
                    new FavoriteController().Create(AddFavorite);
                }

            }
            favorite.Activities = db.activities.ToList();
            favorite.ActivityNames = new SelectList(favorite.Activities, "activityID", "activityName");
            var query = from a in db.activities
                        join b in db.favorites
                        on a.activityID equals b.Activity_activityID
                        where b.Employee_employeeID.Equals("5X67H8")
                        select new FavoriteViewModel { SelectedActivity = a, SelectedFavorite = b };
            favorite.UserFavorites = new SelectList(query.ToArray(), "SelectedFavorite.favoriteID", "SelectedActivity.ActivityName");
            return View("Favorite", favorite);
        }

        // Remove Selected Favorite from Table
        [HttpPost]
        public ActionResult RemoveFavorite(FavoriteViewModel favorite)
        {
            ViewBag.Message = "Your Favorites Page";
            if (favorite != null)
            {
                int[] removeFav = favorite.RemoveFavorites;
                for (int i = 0; i < removeFav.Length; i++)
                {
                    if (removeFav[i] != 0)
                    {
                        new FavoriteController().DeleteConfirmed(removeFav[i]);
                    }
                }

            }
            favorite = new FavoriteViewModel();
            favorite.Activities = db.activities.ToList();
            favorite.ActivityNames = new SelectList(favorite.Activities, "activityID", "activityName");
            var query = from a in db.activities
                        join b in db.favorites
                        on a.activityID equals b.Activity_activityID
                        where b.Employee_employeeID.Equals("5X67H8")
                        select new FavoriteViewModel { SelectedActivity = a, SelectedFavorite = b };
            favorite.UserFavorites = new SelectList(query.ToArray(), "SelectedFavorite.favoriteID", "SelectedActivity.ActivityName");
            return View("Favorite", favorite);
        }
    }
}