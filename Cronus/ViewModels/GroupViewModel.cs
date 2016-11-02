using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseEntities;

namespace Cronus.ViewModels
{
    public class GroupViewModel
    {
        public IEnumerable<project> Projects { get; set; }
        public IEnumerable<employee> Employees { get; set; }
        public IEnumerable<group> Groups { get; set; }
        public group SelectedGroup { get; set; }

    }
}