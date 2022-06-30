using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using RegisterAndLogin6.Models;

namespace RegisterAndLogin6.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                string sqlQuery = "INSERT INTO User (Username, UserId, UserPassword) VALUES(" + user.UserName + "," + user.UserId + "," + user.UserPassword + ")";
                int insert = db.Execute(sqlQuery);
            }

            return View("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new SqlConnection())
                {
                    
                }
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}