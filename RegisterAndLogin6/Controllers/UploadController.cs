using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using RegisterAndLogin6.Models;
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
                        string fileName = Path.GetFileName(file.FileName);
                        //string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                        //string path = @"D:\test\";
                        //string path = @"https:\\localhost:44329\image\";
                        ////string path = @"C:\Users\Srook\source\repos\RegistrationAndLogin7\RegistrationAndLogin7\image\";

                        //file.SaveAs(_path);
                        ////string filePath = path + fileName;
                        ////file.SaveAs(filePath);
                        string filePath2 = @"https:\\localhost:44329\image\" + fileName;

                        //return Json(path);
                        //object paramss = new { Id = Id, ParentId = ParentId, TopId = TopId, Lv = Lv, UserId = Session["UserId"].ToString(), Comment = Comment, Image = filename, Regdate = DateTime.Now };
                        //return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_UploadFile", new { Image = path }, null, true, null, System.Data.CommandType.StoredProcedure));
                        var result = db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_UploadFile", new { Image = filePath2 }, null, true, null, System.Data.CommandType.StoredProcedure);

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
    }
}