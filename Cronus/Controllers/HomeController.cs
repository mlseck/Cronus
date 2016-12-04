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

        [AllowAnonymous]
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
            return PartialView("_hoursworkedrow", myModel);
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
        public JsonResult GetEvents(string empId)
        {
            //create an employee object mathcing the empId passed through
            employee emp = db.employees.Find(empId);
            var dbGrps = db.groups.ToList();
            //all groups where the employee is within the list of employees assigned to the group.
            var groups = (from s in dbGrps where s.employees.Contains(emp) select s).ToList();

            //list of projects to return
            List<project> returnProj = new List<project>();

            //for each group found for that employee
            foreach (group grp in groups)
            {
                group grpObject = db.groups.Find(grp.groupID);
                var dbProjects = db.projects.ToList();
                //you find a list of projects where the project has that group assigned to it. 
                List<project> projects = (from s in dbProjects where s.groups.Contains(grpObject) select s).ToList();

                foreach (project proj in projects)
                {
                    //for each of those projects, add them to the returnProj list. 
                    returnProj.Add(new project()
                    {
                        projectID = proj.projectID,
                        projectName = proj.projectName,
                        projectStartDate = proj.projectStartDate,
                        projectEndDate = proj.projectEndDate
                    });
                }
            }

            //return a list of projects defined above
            return Json(returnProj, JsonRequestBehavior.AllowGet);
        }

        //GetWeeklyHours
        [HttpGet]
        public JsonResult GetWeeklyHours(string empId, int monthOffSet)
        {
            //empId = "Amill";
            DateTime date = DateTime.Now.AddMonths(monthOffSet);
            var StartDate = new DateTime(date.Year, date.Month, 1);
            var EndDate = StartDate.AddMonths(1).AddDays(-1);
            double numDays = (EndDate - StartDate).TotalDays;
            var dailyHours = 0;
            bool isEnd = false;

            List<MonthlyViewModel> hrsWrkd = new List<MonthlyViewModel>();
            var dbHours = db.hoursworkeds.ToList();
            for (int i=0; i < numDays; i++)
            {
                List<hoursworked> hrs = (from s in dbHours where s.TimePeriod_employeeID == empId && s.date == StartDate.AddDays(i) select s).ToList();
                foreach(hoursworked hrsW in hrs)
                {
                    dailyHours = dailyHours + (int)hrsW.hours;
                }
                if (StartDate.AddDays(i) == EndDate.AddDays(-1))
                {
                     isEnd = true;
                }
                else
                {
                    isEnd = false;
                }
                hrsWrkd.Add(new MonthlyViewModel()
                {
                    HrsWorked = dailyHours.ToString(),
                    entryDate = StartDate.AddDays(i),
                    isLastDay = isEnd

                });
                dailyHours = 0;
            }
            return Json(hrsWrkd, JsonRequestBehavior.AllowGet);
    }


        //This will take in a project ID, and EmployeeID, and get the hours worked on each activity for a day
        [HttpGet]
        public JsonResult GetHoursWorkedPerDay(DateTime date, string empId)
        {
            //empId = "Amill";
            //Still need to pass through employee ID, this will do fore now
            // same with date
            //sempId = "Amill";
            //DateTime date = DateTime.Today.AddDays(-1);

            //creates a list of all hours worked for that employee, on the date passed through
            List<hoursworked> hrs = (from s in db.hoursworkeds where s.TimePeriod_employeeID == empId && s.date == date select s).ToList();
            //using this to add each enitity of hours worked to store in a list.
            List<MonthlyViewModel> hrsWrkd = new List<MonthlyViewModel>();

            project proj;
            employee emp;

            //for each set of hours worked we have, we will get a list of activities that were worked on those hours.
            foreach (hoursworked hrsW in hrs)
            {
                
                List<activity> activities = (from s in this.activityRepository.All where s.activityID == hrsW.Activity_activityID select s).ToList();

                //for each of those activitires, we will find the project of the activity worked on, and add  all of 
                //the cooresponding varaibles we need to the HrsWrked list
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

            //return a list of models that hold all the varaibles we need. Each object in the list stores activity name, hrs worked,
            //and project name on the date passed through date. 
            return Json(hrsWrkd, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public JsonResult GetBetweenDates(string empId)
        {

            //empId = "Amill";

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



            List<hoursworked> hrs = (from s in db.hoursworkeds where s.TimePeriod_employeeID == empId && s.date >startDate && s.date < endDate select s).ToList();
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

        //[HttpPost]
        //public JsonResult GetPreviousHours()

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
            HomeViewModel myModel = new HomeViewModel();
            myModel.Projects = db.projects.ToList();
            myModel.Activities = db.activities.ToList();
            return View("Index", myModel);
        }
    }
}