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
            public ICollection<project> Projects { get; set; }
            public ICollection<activity> Activities { get; set; }
            public int SelectedProjectID { get; set; }
            public int SelectedActivityID { get; set; }
            public hoursworked hrsWorked { get; set; }
            public int totalHoursWorked { get; set; }
            public string Comment { get; set; }
            public virtual List<hoursworked> HoursWorked { get; set; }
            public HomeViewModel(){
            HoursWorked = new List<hoursworked>();
            }


    }
}
