using Cronus.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cronus.Controllers
{
    public class LoginController : Controller
    {
        //public ActionResult Login()
        //{

        //    return PartialView("Login", new Logon());
        //}
        [HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Login(Logon logon)
        {

            string status = "The username or password provided is incorrect.";

            // Verify the fields.
            if (ModelState.IsValid)
            {
                // Authenticate the user.
                if (UserManager.ValidateUser(logon, Response))
                {
                    // Redirect to the secure area.
                    return RedirectToAction("Index", "Home");
                    
                    //if (string.IsNullOrWhiteSpace(logon.RedirectUrl))
                    //{
                    //    logon.RedirectUrl = Url.Action("Index", "Home");
                    //}

                    //status = "OK";
                    //string url = Url.Action("Index", "Home");
                    //return Json(new { success = true, url = url });
                    

                    //return Json(new { RedirectUrl = logon.RedirectUrl, Status = status, isRedirect = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return PartialView("Login", new Logon());

            //return Json(new { RedirectUrl = logon.RedirectUrl, Status = status, isRedirect = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Logout()
        {
            // Clear the user session and forms auth ticket.
            UserManager.Logoff(Session, Response);

            return RedirectToAction("Index", "Home");
        }
    }
}

