using DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cronus.Models;
using Cronus.ViewModels;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Data.Entity;

namespace Cronus.Controllers
{
    public class HomeController : Controller
    {
        private CronusDatabaseEntities db = new CronusDatabaseEntities();
        HomeViewModel myModel = new HomeViewModel();


        private readonly IProjectRepository projectRepository;
        private readonly IActivityRepository activityRepository;



        // If you are using Dependency Injection, you can delete the following constructor
        public HomeController() : this(new ProjectRepository(), new ActivityRepository())
        {
        }

        public HomeController(IProjectRepository projectRepository, IActivityRepository activityRepository)
        {
            this.projectRepository = projectRepository;
            this.activityRepository = activityRepository;
        }

        public ActionResult Index()
        {
            HomeViewModel myModel = new HomeViewModel();
            myModel.Projects = db.projects.ToList();
            myModel.Activities = db.activities.ToList();
            myModel.HoursWorked = db.hoursworkeds.ToList();
            return View(myModel);

        }


        public ActionResult AddHourWorked()
        {
            HomeViewModel myModel = new HomeViewModel();
            myModel.Projects = db.projects.ToList();
            myModel.Activities = db.activities.ToList();
            myModel.HoursWorked = db.hoursworkeds.ToList();
            return PartialView("_hoursworkedrow", myModel.hrsWorked);
        }



        //Method will update the activities dropdownlist in index page when specific project is picked
        [HttpPost]
        public ActionResult FillActivities(int projectID)
        {
            List<activity> objactivity = new List<activity>();
            objactivity = db.projects.Find(projectID).activities.ToList();
            SelectList myActivities = new SelectList(objactivity, "activityID", "activityName", 0);
            return Json(myActivities);
        }

        public ActionResult Monthly()
        {
            MonthlyViewModel monthlyModel = new MonthlyViewModel();
            return View(monthlyModel);
        }


        //gets projects for the calender 
        public JsonResult GetEvents()
        {
            //will get projects/activities for the month.
            List<project> projects = new List<project>();
            foreach (project proj in db.projects)
            {
                projects.Add(new project()
                {
                    projectID = proj.projectID,
                    projectName = proj.projectName,
                    projectStartDate = proj.projectStartDate,
                    projectEndDate = proj.projectEndDate
                });
            }
            return Json(projects, JsonRequestBehavior.AllowGet);
        }

        //This will take in a project ID, and EmployeeID, and get the hours worked on each activity for a day
        [HttpGet]
        public JsonResult GetHoursWorkedPerDay(DateTime date)
        {
            //Still need to pass through employee ID, this will do fore now
            // same with date
            string empId = "Amill";
            //DateTime date = DateTime.Today.AddDays(-1);

            List<hoursworked> hrs = (from s in db.hoursworkeds where s.TimePeriod_employeeID == empId && s.date == date select s).ToList();
            List<MonthlyViewModel> hrsWrkd = new List<MonthlyViewModel>();

            project proj;
            employee emp;

            foreach (hoursworked hrsW in hrs)
            {
                List<activity> activities = (from s in this.activityRepository.All where s.activityID == hrsW.Activity_activityID select s).ToList();

                foreach (activity activ in activities)
                {
                    proj = db.projects.Find(hrsW.Project_projectID);
                    emp = db.employees.Find(empId);
                    hrsWrkd.Add(new MonthlyViewModel()
                    {
                        ActivityName = activ.activityName,
                        HrsWorked = hrsW.hours.ToString(),
                        ProjectName = proj.projectName,
                        //isAdmin = 
                    });
                }
            }

           
            return Json(hrsWrkd, JsonRequestBehavior.AllowGet);
        }

        //this wil lget hours per week per employee
        [HttpGet]
        public JsonResult GetHoursWorkedPerWeek(WeeklyHoursModel model, string empId)
        {
            //Still need to pass through employee ID, this will do fore now
            // same with date
            empId = "Amill";
            //DateTime date = DateTime.Today.AddDays(-1);

            List<hoursworked> hrs = (from s in db.hoursworkeds where s.TimePeriod_employeeID == empId select s).ToList();
            List<MonthlyViewModel> hrsWrkd = new List<MonthlyViewModel>();

            project proj;
            employee emp;

            foreach (hoursworked hrsW in hrs)
            {
                List<activity> activities = (from s in this.activityRepository.All where s.activityID == hrsW.Activity_activityID select s).ToList();

                foreach (activity activ in activities)
                {
                    proj = db.projects.Find(hrsW.Project_projectID);
                    emp = db.employees.Find(empId);
                    hrsWrkd.Add(new MonthlyViewModel()
                    {
                        ActivityName = activ.activityName,
                        HrsWorked = hrsW.hours.ToString(),
                        ProjectName = proj.projectName,
                        //isAdmin = 
                    });
                }
            }


            return Json(hrsWrkd, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult GetBetweenDates(String employeeID)
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


            //List<MonthlyViewModel> hrsWrkd = new List<MonthlyViewModel>();
            //foreach (hoursworked hrs in db.hoursworkeds
            //                                    .Where(n => n.date>= startDate)
            //                                    .Where(n => n.date<= endDate)
            //                                    .Where(n=> n.Employee_employeeID == employeeID))
            //{
            //    hrsWrkd.Add(new MonthlyViewModel()
            //    {
            //        HrsWorked = hrs.hours.ToString()
            //    });
            //}

            //var getHoursQuery = from hrs in db.hoursworkeds
            //                    where (hrs.TimePeriod_employeeID == "Amill" &&
            //                    hrs.date >= startDate &&
            //                    hrs.date <= endDate)
            //                    select hrs ;
            //homeModel.HoursWorked = getHoursQuery.ToList();

            //JsonConvert.SerializeObject(homeModel, Formatting.Indented,
            //                new JsonSerializerSettings
            //                {
            //                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //                });

            
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
                        where b.Employee_employeeID.Equals("TestID")
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
                AddFavorite.Employee_employeeID = "TestID";
                // Check if selected activity is already a favorite. If not, add to Favorite Table
                var existsQuery = from f in db.favorites
                                  where (f.Activity_activityID.Equals(AddFavorite.Activity_activityID) && f.Employee_employeeID.Equals("TestID"))
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
                        where b.Employee_employeeID.Equals("TestID")
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
                        where b.Employee_employeeID.Equals("TestID")
                        select new FavoriteViewModel { SelectedActivity = a, SelectedFavorite = b };
            favorite.UserFavorites = new SelectList(query.ToArray(), "SelectedFavorite.favoriteID", "SelectedActivity.ActivityName");
            return View("Favorite", favorite);
        }

        [HttpPost]
        public ActionResult SubmitHours(HomeViewModel submittedHours)
        {
            foreach(var entry in submittedHours.HoursWorked)
            {
                if (entry.hours > 0 )
                {
                    if (entry.entryID == 0)
                    {
                        // Check if the activity was already logged on the current date
                        // If it was, just add hours to the entry
                        // Otherwise, create a new entry
                        hoursworked newEntry = new hoursworked();
                        DateTime periodEndDate = ExtensionMethods.Next(DateTime.Now, DayOfWeek.Sunday);
                        newEntry.hours = entry.hours; newEntry.Project_projectID = entry.Project_projectID; newEntry.Activity_activityID = entry.Activity_activityID; newEntry.date = ExtensionMethods.GetDateInWeek(periodEndDate, entry.currentDay);
                        newEntry.comments = entry.comments; newEntry.TimePeriod_employeeID = "Amill"; newEntry.TimePeriod_periodEndDate = periodEndDate.Date;

                        if (ExtensionMethods.EntryExists(newEntry) == null)
                        {
                            try
                            {
                                new HoursWorkedController().Create(newEntry);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            hoursworked existingEntry = ExtensionMethods.EntryExists(newEntry);
                            existingEntry.hours += newEntry.hours;
                            existingEntry.comments += newEntry.comments;
                            new HoursWorkedController().AddHours(existingEntry);
                        }
                    }
                    // Updating already existing entry
                    else
                    {
                        // If we get to this point, entry is already saved to DB, and we're editing it
                        hoursworked existingEntry = db.hoursworkeds.First(hw => hw.entryID == entry.entryID);
                        existingEntry.Activity_activityID = entry.Activity_activityID; existingEntry.activity = entry.activity;
                        existingEntry.Project_projectID = entry.Project_projectID; existingEntry.project = entry.project;
                        existingEntry.hours = entry.hours;
                        existingEntry.comments = entry.comments;
                        existingEntry.date = entry.date;
                        new HoursWorkedController().AddHours(existingEntry); 
                    }
                }
            }
            HomeViewModel myModel = new HomeViewModel();
            myModel.Projects = db.projects.ToList();
            myModel.Activities = db.activities.ToList();
            myModel.HoursWorked = db.hoursworkeds.ToList();
            return View("Index", myModel);
        }

        
    }

    public static class ExtensionMethods{
        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }

        public static DateTime GetDateInWeek(this DateTime end, DayOfWeek dayOfWeek)
        {
            int day = (int)dayOfWeek;
            if (day == 0) { day = 7; }
            DateTime date = end.Date.AddDays(-(7-day));
            return date;
        }
        public static hoursworked EntryExists(hoursworked entry)
        {
            var existsQuery = from hw in new CronusDatabaseEntities().hoursworkeds
                              where (hw.TimePeriod_employeeID.Equals(entry.TimePeriod_employeeID) && DbFunctions.TruncateTime(hw.date) == DbFunctions.TruncateTime(entry.date) && hw.Project_projectID == entry.Project_projectID && hw.Activity_activityID == entry.Activity_activityID)
                              select hw;
           if(existsQuery.Any())
            {
                return existsQuery.First();
            }
            return null;
        }
    }
}