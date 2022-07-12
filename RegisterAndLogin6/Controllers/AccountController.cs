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
using RegisterAndLogin6.Models;

namespace RegisterAndLogin6.Controllers
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
                string sql = @"INSERT INTO [dbo].[Account] ([UserId], [Email], [Password]) VALUES (@UserId, @Email, @Password)";
                //var result = db.Execute(registerQuery, account);
                db.Execute(sql, account);
            }
            return RedirectToAction("Login", "Account");
        }

        /* 회원가입 시 아이디 중복확인 */
        public string CheckUserId(string UserId)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string sql = @"SELECT * FROM Account WHERE UserId = @UserId";
                var status = db.Query<Account>(sql, new { UserId = UserId }).FirstOrDefault();
                if (status != null)
                {
                    return "1"; //이미 DB에 있는 아이디 (중복된 아이디)
                }
                else
                {
                    return "0"; //DB에 없는 아이디 (사용 가능한 아이디)
                }
            }
        }

        /*[System.Web.Services.WebMethod]
        public static string CheckUserId(string UserId)
        {
            string result = "";
            SqlConnection con = new SqlConnection("Server=localhost;User=test;Password=1234;Database=test;");
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Account WHERE UserId = @UserId", con);
            cmd.Parameters.AddWithValue("@UserId", UserId);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                result = "true";
            }
            else
            {
                result = "false";
            }
            return result;
        }*/

        /*[HttpPost]
        public JsonResult doesUserNameExist(string UserId)
        {

            var user = Membership.GetUser(UserId);

            return Json(user == null);
        }*/

        /*[System.Web.Services.WebMethod]
        public static bool CheckUserName(string UserId)
        {
            bool status = false;
            string constr = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("CheckUserAvailability", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", UserId.Trim());
                    conn.Open();
                    status = Convert.ToBoolean(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
            return status;
        }*/

        /*[HttpPost]
        public JsonResult CheckUserId(string UserId)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                //bool isValid = !db.Users.ToList().Exists(p => p.UserId.Equals(UserId, StringComparison.CurrentCultureIgnoreCase));
                var sql = "SELECT * FROM ACCOUNT (nolock) WHERE UserId = @UserId";
                bool isValid = !db.QueryFirstOrDefault(sql).ToList();
                return Json(isValid);
            }
        }*/

        /*[HttpPost]
        public ActionResult Register(Account account)
        {
            using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                if (ModelState.IsValid)
                {
                    db.Open();
                    if ((db.State & System.Data.ConnectionState.Open) > 0)
                    {
                        SqlCommand cmd = new SqlCommand("UserRegistration", db);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserId", account.UserId));
                        cmd.Parameters.Add(new SqlParameter("@Email", account.Email));
                        cmd.Parameters.Add(new SqlParameter("@Password", account.Password));
                        int results = cmd.ExecuteNonQuery();
                    }
                    db.Close();
                    return View();
                }
                else
                {
                    return View();
                }

            }
        }*/


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserId, string Password)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string result = "Fail";
                string sql = "SELECT * FROM Account WHERE UserId = @UserId AND Password = @Password";
                var data = db.Query<Account>(sql, new { UserId = UserId, Password = Password }).FirstOrDefault();
                //db.QueryFirst<Account>(sql, new { UserId = UserId, Password = Password });

                if (data != null)
                {
                    Session["UserId"] = data.UserId.ToString();
                    result = "Success";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /*[HttpPost]
        public ActionResult Login(Account account)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
                {
                    var sql = @"SELECT * FROM Account (nolock) WHERE UserId = @UserId AND Password = @Password";
                    var user = db.Query<Account>(sql, new { UserId = account.UserId, Password = account.Password }).FirstOrDefault();
                    if (user != null)
                    {
                        Session["UserId"] = Convert.ToString(account.UserId);
                        return RedirectToAction("Welcome", "Home");
                    }
                }
            }
            ModelState.AddModelError(string.Empty, "사용자 ID 혹은 비밀번호가 올바르지 않습니다.");
            return View(account);
        }*/


        /*[HttpPost]
        public ActionResult Login(Account account)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
                {
                    var sql = @"SELECT * FROM Account (nolock) WHERE UserId = @UserId AND Password = @Password";
                    var obj = db.Query<Account>(sql, new { UserId = account.UserId, Password = account.Password }).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["Id"] = obj.Id.ToString();
                        Session["UserId"] = obj.UserId.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(account);
        }*/

        /*[HttpPost]
        public JsonResult ValidateAccount(string UserId, string Password)
        {
            using (var connection = new SqlConnection("Server=localhost;User=test;Password=1234;Database=test;"))
            {
                var data = @"SELECT * FROM Account WHERE UserId = @UserId AND Password = @Password";
                if (data.Count() > 0)
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
            }
        }*/

        /*[HttpPost]
        public ActionResult LoginUser(Account account)
        {
            using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string sql = @"SELECT * FROM Account WHERE UserId = @UserId AND Password = @Password";
                var status = db.Query<Account>(sql, new { UserId = account.UserId, Password = account.Password }).FirstOrDefault();
                if (status != null)
                {
                    Session["UserId"] = account.UserId;
                    Session.Timeout = 10;
                    return RedirectToAction("Welcome", "Home");
                }
                else
                {
                    return View();
                }
            }
        }*/




        /*public ActionResult CheckAuthentication(string UserId, string Password)
        {
            using (SqlConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string sql = @"SELECT * FROM Account WHERE UserId = @UserId AND Password = @Password";
                int count = db.Query<Account>(sql, new { UserId = UserId, Password = Password }).ToList().Count;
                if (count == 1)
                {
                    return Json("Authenticated", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Failed");
                }
            }
        }*/

        /*[HttpPost]
        public ActionResult Login(Account account, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }
            else
            {
                return View();
            }
        }*/

        /*[HttpPost]
        public ActionResult Login(Account account, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(account.UserId, account.Password))
                {
                    FormsAuthentication.SetAuthCookie(account.UserId, account.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "아이디 또는 비밀번호를 다시 한번 확인해주세요.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(account);
        }*/


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

        /*[HttpPost]
        public ActionResult Login(Account account)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string loginQuery = @"SELECT * FROM [dbo].[Account] WHERE UserId = @UserID AND Password = @Password";
                db.QueryFirstOrDefault(loginQuery, account);
            }
            return RedirectToAction("Welcome", "Home");
        }*/

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

        // 로그아웃
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        /*public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }*/

        /*public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }*/

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


        /* 내 정보 */
        public ActionResult MyInfo()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        /* 비밀번호 변경 */
        public ActionResult ChangePassword()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("성공적으로 비밀번호를 변경하였습니다.");
                }
                else
                {
                    ModelState.AddModelError("", "입력하신 비밀번호가 올바르지 않습니다. 다시 한번 확인해주세요.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        // 회원삭제
        public void Delete(int? Id)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string deleteQuery = "DELETE FROM Account WHERE Id = @Id";
                db.Execute(deleteQuery, new { Id = Id });
            }
        }
    }
}