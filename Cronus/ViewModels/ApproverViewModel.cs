using DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cronus.ViewModels
{
    public class ApproverViewModel
    {
        public IEnumerable<project> Projects { get; set; }
        public IEnumerable<activity> Activities { get; set; }
        public IEnumerable<employee> Employees { get; set; }
        public ICollection<employeetimeperiod> employeetimeperiods { get; set; }
        public ICollection<hoursworked> HoursWorkedList { get; set; }
        public timeperiod timeperiod { get; set; }
        public bool isApproved { get; set; }

    }
}