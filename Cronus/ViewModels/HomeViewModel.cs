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
            public List<project> ProjectList { get; set; }
            public ICollection<hoursworked> HoursWorked { get; set; }
            public hoursworked hrsWorked { get; set; }
            public DateTime currentWeekEndDate { get; set; }
            public bool isApproved { get; set; }
    }   
}
