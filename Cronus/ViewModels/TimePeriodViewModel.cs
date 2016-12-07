using DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cronus.ViewModels
{
    public class TimePeriodViewModel
    {

        public DateTime periodEndDate { get; set; }
        public List<SelectListItem> timePeriods;
    }
}