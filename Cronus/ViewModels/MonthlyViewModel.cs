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


        public String Name { get; set; }
        public String StartDate { get; set; }
        public DateTime endMonth { get; set; }
        public DateTime entryDate { get; set; }
        public Boolean isLastDay { get; set; }


        public String ActivityName { get; set; }
        public String HrsWorked { get; set; }
        public String ProjectName { get; set; }
        public bool isAdmin { get; set; }
    }
}