using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cronus.Login
{
    public class Logon
    {
        [Required]
        public string EmployeeID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string RedirectUrl { get; set; }

        public string ErrorMessage { get; set; }
    }
}