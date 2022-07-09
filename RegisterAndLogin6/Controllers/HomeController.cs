using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegisterAndLogin6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // Dapper CRUD Board
        public ActionResult Board()
        {
            ViewBag.Message = "자유게시판";

            return View();
        }

        // 로그인 후 welcome 페이지
        public ActionResult Welcome()
        {
            return View();
        }
    }
}