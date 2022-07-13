using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Dapper;
using RegisterAndLogin6.Models;

namespace RegisterAndLogin6.Controllers
{
    public class AccountController : Controller
    {
        /* 회원가입: GET */
        public ActionResult Register()
        {
            return View();
        }

        /* 회원가입: POST */
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

        /* 로그인: GET */
        public ActionResult Login()
        {
            return View();
        }

        /* 로그인: POST */
        [HttpPost]
        public ActionResult Login(string UserId, string Password)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string result = "Fail";
                string sql = "SELECT * FROM Account WHERE UserId = @UserId AND Password = @Password";
                var data = db.Query<Account>(sql, new { UserId = UserId, Password = Password }).FirstOrDefault();
                if (data != null)
                {
                    Session["UserId"] = data.UserId.ToString();
                    result = "Success";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /* 로그아웃 */
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

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

        /* 비밀번호 찾기: GET */
        public ActionResult ForgetPassword()
        {
            return View();
        }

        /* 비밀번호 찾기: POST */
        [HttpPost]
        public ActionResult ForgetPassword(Account account)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                var IsExists = IsEmailExists(account.Email);
                if (!IsExists)
                {
                    ModelState.AddModelError("EmailNotExists", "해당 이메일 주소가 존재하지 않습니다.");
                    return View();
                }
                string sql = @"SELECT * FROM Account WHERE Email = @Email";
                var objUsr = db.Query<Account>(sql, new { Email = account.Email }).FirstOrDefault();

                // Genrate OTP     
                string OTP = GenerateOTP();

                objUsr.ActivationCode = Guid.NewGuid();
                objUsr.OTP = OTP;
                objCon.Entry(objUsr).State = System.Data.Entity.EntityState.Modified;
                objCon.SaveChanges();

                ForgetPasswordEmailToUser(objUsr.Email, objUsr.ActivationCode.ToString(), objUsr.OTP);
                var Message = "발송된 이메일 내 URL를 클릭 후 발급된 OTP 인증번호와 함께 새로운 비밀번호를 입력해주세요. => " + objUsr.Email;
                ViewBag.Message = Message;

                return View("FindPassword");
            }
        }

        // 이메일 중복체크
        public bool IsEmailExists(string Email)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                string sql = @"SELECT * FROM Account WHERE Email = @Email";
                var IsCheck = db.Query<Account>(sql, new { Email = Email }).FirstOrDefault();
                return IsCheck != null;
            }
        }

        // OTP 생성
        public string GenerateOTP()
        {
            string OTPLength = "4";
            string OTP = string.Empty;

            string Chars = string.Empty;
            Chars = "1,2,3,4,5,6,7,8,9,0";

            char[] splitChar = { ',' };
            string[] arr = Chars.Split(splitChar);
            string NewOTP = "";
            string temp = "";
            Random random = new Random();
            for (int i = 0; i < Convert.ToInt32(OTPLength); i++)
            {
                temp = arr[random.Next(0, arr.Length)];
                NewOTP += temp;
                OTP = NewOTP;
            }
            return OTP;
        }

        // 비밀번호 변경시 SMTP
        public void ForgetPasswordEmailToUser(string emailId, string activationCode, string OTP)
        {
            var GenerateUserVerificationLink = "/Register/ChangePassword/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenerateUserVerificationLink);
            var fromMail = new MailAddress("lim950808@naver.com", "Srook");
            var fromEmailpassword = "Dlawodjr@8@8";
            var toEmail = new MailAddress(emailId);

            var smtp = new SmtpClient();
            smtp.Host = "smtp.naver.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

            var Message = new MailMessage(fromMail, toEmail);
            Message.Subject = "비밀번호 변경을 위한 OTP 인증번호 발급";
            Message.Body = "<br/> 아래의 링크에 들어가 비밀번호 변경을 완료해주세요." +
                            "<br/><br/><a href=" + link + ">" + link + "</a>" +
                            "<br/> OTP 인증번호 : " + OTP;
            Message.IsBodyHtml = true;
            smtp.Send(Message);
        }

        // 회원가입 이메일 인증시 SMTP
        public void SendEmailToUser(string emailId, string activationCode)
        {
            var GenerateUserVerificationLink = "/Register/UserVerification/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenerateUserVerificationLink);

            var fromMail = new MailAddress("lim950808@naver.com", "스룩페이"); // 보내는 사람(관리자, admin) 이메일  
            var fromEmailpassword = "Dlawodjr@8@8"; // 보내는 사람(관리자, admin) 비밀번호     
            var toEmail = new MailAddress(emailId); // 받는 사람 이메일

            var smtp = new SmtpClient();
            smtp.Host = "smtp.naver.com"; // 네이버 메일. Gmail은 2022.05.30일부터 "보안 수준이 낮은 앱" 서비스 종료.
            smtp.Port = 587; // port는 naver랑 Gmail 모두 587
            smtp.EnableSsl = true; // SSL(Secure Sockets Layer)를 이용하여 연결을 암호화 할지 여부
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword);

            var Message = new MailMessage(fromMail, toEmail);
            Message.Subject = "회원가입 이메일 인증";
            Message.Body = "<br/> 회원가입이 성공적으로 완료되었습니다." +
                           "<br/> 아래의 링크를 눌러 이메일 인증을 해주세요." +
                           "<br/><br/><a href=" + link + ">" + link + "</a>";
            Message.IsBodyHtml = true; // IsBodyHtml : 전자 메일 메시지 본문의 형식이 HTML인지 여부
            smtp.Send(Message); // 예) https://localhost:44343/Register/UserVerification/8782e81a-99d6-4565-910b-f422260b4c70
        }

        // 회원가입 후 이메일 인증
        public ActionResult UserVerification(string id)
        {
            bool Status = false;

            objCon.Configuration.ValidateOnSaveEnabled = false; //SaveChanges를 호출할 때 추적된 엔터티의 유효성을 자동으로 검사해야 하는지 여부. 기본값은 true.
            var IsVerify = objCon.UserMs.Where(u => u.ActivetionCode == new Guid(id)).FirstOrDefault();

            if (IsVerify != null)
            {
                IsVerify.EmailVerification = true;
                objCon.SaveChanges();
                ViewBag.Message = "이메일 인증이 완료되었습니다.";
                Status = true;
            }
            else
            {
                ViewBag.Message = "유효하지 않은 요청. 이메일 인증 실패!";
                ViewBag.Status = false;
            }
            return View();
        }

        /* 비밀번호 변경: GET */
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

        /* 비밀번호 변경: POST */
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

        /* 회원 탈퇴: Get */
        public ActionResult Quit()
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

        /* 회원 탈퇴: POST */
        [HttpPost]
        public ActionResult Quit(Account account)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString))
            {
                account.UserId = Session["UserId"].ToString();
                string sql = "DELETE FROM Account WHERE UserId = @UserId";
                db.Execute(sql, account);
            }
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}