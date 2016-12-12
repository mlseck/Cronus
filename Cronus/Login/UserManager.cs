using DatabaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Script.Serialization;


namespace Cronus.Login
{
    public static class UserManager
    {
        /// <summary>
        /// Returns the User from the Context.User.Identity by decrypting the forms auth ticket and returning the user object.
        /// </summary>
        public static employee User
        {
            get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    // The user is authenticated. Return the user from the forms auth ticket.
                    return ((MyPrincipal)(HttpContext.Current.User)).User;
                }
                else if (HttpContext.Current.Items.Contains("User"))
                {
                    // The user is not authenticated, but has successfully logged in.
                    return (employee)HttpContext.Current.Items["User"];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Authenticates a user against a database, web service, etc.
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>User</returns>
        public static employee AuthenticateUser(string username, string password, CronusDatabaseEntities db)
        {
            employee user = null;

            // Lookup user in database, web service, etc. We'll just generate a fake user for this demo.

            var employeeIDList = db.employees.Select(x => x.employeeID).ToList();
            if (employeeIDList.Contains(username))
            {
                user = db.employees.Find(username);

                //bool isManager = db.groups.Select(g => g).Where(g => g.groupManager.Equals(user.employeeID)).Any();
                int? employeeManages = null;

                group group = db.groups.Select(g => g).Where(g => g.groupManager.Equals(user.employeeID)).FirstOrDefault();

                if (group != null)
                {
                    employeeManages = group.groupID;
                }

                if (user.employeePwd == password)
                {
                    user = new employee { employeeID = user.employeeID, employeeFirstName = user.employeeFirstName,
                        employeeLastName = user.employeeLastName, employeePrivileges = user.employeePrivileges,
                        managesgroup = employeeManages/*, isManager = isManager*/};
                    return user;
                }

                return null;
            }
            //if (username == "abel" && password == "abel")
            //{
            //    user = new employee { employeeID = "125", employeeFirstName = "Abel", employeeLastName = "Teferra" };
            //}

            return user;
        }

        /// <summary>
        /// Authenticates a user via the MembershipProvider and creates the associated forms authentication ticket.
        /// </summary>
        /// <param name="logon">Logon</param>
        /// <param name="response">HttpResponseBase</param>
        /// <returns>bool</returns>
        public static bool ValidateUser(Logon logon, HttpResponseBase response)
        {
            bool result = false;

            if (Membership.ValidateUser(logon.EmployeeID, logon.Password))
            {
                // Create the authentication ticket with custom user data.
                var serializer = new JavaScriptSerializer();
                string userData = serializer.Serialize(UserManager.User);

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        logon.EmployeeID,
                        DateTime.Now,
                        DateTime.Now.AddDays(30),
                        true,
                        userData,
                        FormsAuthentication.FormsCookiePath);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                result = true;
            }

            return result;
        }

        /// <summary>
        /// Clears the user session, clears the forms auth ticket, expires the forms auth cookie.
        /// </summary>
        /// <param name="session">HttpSessionStateBase</param>
        /// <param name="response">HttpResponseBase</param>
        public static void Logoff(HttpSessionStateBase session, HttpResponseBase response)
        {
            // Delete the user details from cache.
            session.Abandon();

            // Delete the authentication ticket and sign out.
            FormsAuthentication.SignOut();

            // Clear authentication cookie.
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            response.Cookies.Add(cookie);
        }
    }
}