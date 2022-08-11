using System;
using System.Collections.Generic;
using System.IO;  //파일과 데이터 스트림에 대한 읽기 및 쓰기를 허용하는 형식
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using RegisterAndLogin6.Models;
using RegisterAndLogin6.Models.SPA;

namespace RegisterAndLogin6.Controllers
{
    public class UploadController : Controller
    {
        /* DB 연결 */
        public string DbConnection = "Data Source=localhost;Initial Catalog=test;User Id=test;Password=1234";

        // GET: Upload  
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                    {
                        string fileName = Path.GetFileName(file.FileName); //Path: 파일이나 디렉터리 경로 정보
                        string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                        //string path = @"D:\test\";
                        //string path = @"https:\\localhost:44329\image\";
                        ////string path = @"C:\Users\Srook\source\repos\RegistrationAndLogin7\RegistrationAndLogin7\image\";

                        file.SaveAs(_path);
                        ////string filePath = path + fileName;
                        ////file.SaveAs(filePath);
                        string filePath2 = @"https:\\localhost:44329\image\" + fileName;

                        //return Json(path);
                        //object paramss = new { Id = Id, ParentId = ParentId, TopId = TopId, Lv = Lv, UserId = Session["UserId"].ToString(), Comment = Comment, Image = filename, Regdate = DateTime.Now };
                        //return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_UploadFile", new { Image = path }, null, true, null, System.Data.CommandType.StoredProcedure));
                        db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_UploadFile", new { Image = filePath2 }, null, true, null, System.Data.CommandType.StoredProcedure);

                    }
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }

        [HttpPost]
        public ActionResult getCommentList(CommentList commentlist)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                string sql = "SELECT * FROM CommentList (nolock) ORDER BY Id DESC";
                ViewBag.upload = db.Query<Models.SPA.CommentList>(sql, new { Id = commentlist.Id, UserId = Session["UserId"].ToString(), Comment = commentlist.Comment, Regdate = DateTime.Now }).ToList();
            }
            return View();
        }

        [HttpGet]
        public ActionResult UploadFile1()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile1(HttpPostedFileBase file)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                string fileName = Path.GetFileName(file.FileName);
                string filePath = @"https:\\localhost:44329\image\" + fileName;
                db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_UploadFile", new { Image = filePath }, null, true, null, System.Data.CommandType.StoredProcedure);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string id)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                file.SaveAs(path);
            }
            return RedirectToAction("UploadFile");
        }
    }
}