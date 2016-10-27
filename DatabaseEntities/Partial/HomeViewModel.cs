using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntities
{
    public class HomeViewModel
    {
            public IEnumerable<project> Projects { get; set; }
            public IEnumerable<activity> Activities { get; set; }
            public IEnumerable<favorite> Favorites { get; set; }

            //Working on pulling hours/activities based on week and employee
            public IEnumerable<hoursworked> HoursWorked { get; set; }
            public hoursworked hrsWorked { get; set; }
         
    }
}
