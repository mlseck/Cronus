using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseEntities;
using System.Web.Mvc;

namespace Cronus.ViewModels
{
    public class HomeViewModel
    {
            public IEnumerable<project> Projects { get; set; }
            public IEnumerable<activity> Activities { get; set; }
            public IEnumerable<favorite> Favorites { get; set; }
            public SelectList ProjectList { get; set; }
            public SelectList ActivityList { get; set; }
            public int SelectedProjectID { get; set; }
            public int SelectedActivityID { get; set; }

            //Working on pulling hours/activities based on week and employee
            public IEnumerable<hoursworked> HoursWorked { get; set; }
            public hoursworked hrsWorked { get; set; }
            public int totalHoursWorked { get; set; }
            public string Comment { get; set; }
         
    }
}
