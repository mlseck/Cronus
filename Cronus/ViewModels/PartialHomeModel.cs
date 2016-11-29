using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cronus.ViewModels
{
    public class PartialHomeModel
    {
        public SelectList ProjectList { get; set; }
        public SelectList ActivityList { get; set; }
        public int SelectedProjectID { get; set; }
        public int SelectedActivityID { get; set; }
    }
}