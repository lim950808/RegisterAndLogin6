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
        public ActionResult Register(Account account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
                    {
                        string registerQuery = "INSERT INTO dbo.Account (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                        var result = connection.Execute(registerQuery, account);

                        return RedirectToAction("Index");
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "회원가입 불가");
            }
            return View(account);
        }

        public ActionResult Login(Account account)
        {
            if (ModelState.IsValid)
            {
                /*using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString))
                {
                    var user = db.account.FirstOrDefault(u => )
                }*/
                return RedirectToAction("Index", "Home");
            }
            else
            {

            }

            return View();
        }
    }
}