using DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cronus.ViewModels
{
    public class WeeklyHoursModel
    {

        public IEnumerable<hoursworked> HoursWorked { get; set; }
        public String HrsWorked { get; set; }
    }
}