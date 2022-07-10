using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Dapper;
using RegistrationAndLogin6.Models;

namespace RegistrationAndLogin7.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Account account)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string registerQuery = @"INSERT INTO [dbo].[Account2] ([UserId], [Email], [Password]) VALUES (@UserId, @Email, @Password)";
                //var result = db.Execute(registerQuery, account);
                db.Execute(registerQuery, account);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Login()
        {
            return View();
        }

        /*[HttpPost]
        public ActionResult Login(string UserId, string Password)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string loginQuery = "SELECT * FROM [dbo].[Account] WHERE UserId = @UserID AND Password = @Password";
                db.QueryFirstOrDefault<Account>(loginQuery, new { UserId = 1 });
            }
            return RedirectToAction("Welcome", "Home");
        }*/

        [HttpPost]
        public ActionResult Login(Account account)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string loginQuery = @"SELECT * FROM [dbo].[Account2] WHERE UserId = @UserID AND Password = @Password";
                db.QueryFirstOrDefault(loginQuery, account);
            }
            return RedirectToAction("Welcome", "Home");
        }

        /*public JsonResult CheckLogin(Account account)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string loginQuery = @"SELECT * FROM Account WHERE UserId = @UserId AND Password = @Password";
                var dataItem = db.QuerySingleOrDefault(loginQuery, account);
                bool isLogged = true;
                if (dataItem != null)
                {
                    FormsAuthentication.SetAuthCookie(account.UserId, true);
                    var mdl = System.Web.HttpContext.Current.User.Identity.Name;
                    isLogged = true;
                }
                else
                {
                    isLogged = false;
                }
                return Json(isLogged, JsonRequestBehavior.AllowGet);
            }
        }*/


        /*[HttpPost]
        public JsonResult ValidateAccount(string UserId, string Password, bool RememberMe)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                LoginStatus status = new LoginStatus();
                if (Membership.ValidateUser(UserId, Password))
                {
                    FormsAuthentication.SetAuthCookie(UserId, RememberMe);
                    status.Success = true;
                    status.TargetURL = FormsAuthentication.GetRedirectUrl(UserId, RememberMe);
                    if (string.IsNullOrEmpty(status.TargetURL))
                    {
                        status.TargetURL = FormsAuthentication.DefaultUrl;
                    }
                    status.Message = "로그인이 성공적으로 완료되었습니다.";
                }
                else
                {
                    status.Success = false;
                    status.Message = "유효하지 않은 아이디 또는 비밀번호 입니다.";
                    status.TargetURL = FormsAuthentication.LoginUrl;
                }
                return Json(status);
            }
        }*/

        /*[HttpPost]
        public ActionResult Login(Account account)
        {
            if (ModelState.IsValid)
            {
                using (IDbConnection db = new SqlConnection("Server=localhost;User=test;Password=1234;Database=test;"))
                {
                    var userLoggedIn = db.Execute();
                    if (userLoggedIn != null)
                    {
                        return RedirectToAction("Index", "Home", new { UserId = account.UserId });
                    }
                    else
                    {
                        return View();
                    }
                }
               return RedirectToAction("Index", "Home");
            }
            else
            {
            }
            return View();
        }*/

        // 로그아웃
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AuthenticationManager.Logout(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }*/
        /*public ActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Login");
        }*/

        // 회원삭제
        public void Delete(int Id)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string deleteQuery = "DELETE Account2 WHERE Id = @Id";
                db.Execute(deleteQuery, new { Id = Id });
            }
        }
    }
}