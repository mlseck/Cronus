using DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cronus.ViewModels
{
    public class MonthlyViewModel
    {
        public IEnumerable<project> Projects { get; set; }
        public IEnumerable<activity> Activities { get; set; }
        public IEnumerable<hoursworked> HoursWorked { get; set; }

        public project prjct { get; set; }

        public String Name { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }


        public String ActivityName { get; set; }
        public String HrsWorked { get; set; }
    }
}