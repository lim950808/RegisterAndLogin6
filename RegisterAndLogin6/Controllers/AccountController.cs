using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using RegistrationAndLogin6.Models;

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
        public ActionResult Register(Account account)
        {
            /*using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                string sqlQuery = "INSERT INTO Account (Username, UserId, UserPassword) VALUES(" + account.Name + "," + account.Email + "," + account.Password + ")";
                int insert = db.Execute(sqlQuery);
            }*/
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
            {
                string str_query = "INSERT INTO Account(Id, Name, Email, Password) VALUES (@Id, @Name, @Email, @Password)";
                connection.Execute(str_query, account);
            }

            return View("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account account)
        {
            if (ModelState.IsValid)
            {
                /*using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
                {
                    var user = db.User.FirstOrDefault(u => )
                }*/
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}