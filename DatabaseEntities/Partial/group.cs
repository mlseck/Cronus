using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseEntities
{
    public partial class group
    {
        public string[] employeeIds { get; set; }
        public int[] projectIds { get; set; }

    }
}
