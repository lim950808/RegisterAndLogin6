﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using RegisterAndLogin6.Models;
using RegisterAndLogin6.Models.SPA;

namespace RegisterAndLogin6.Controllers
{
    public class BoardController : Controller
    {
        /* DB 연결 */
        public string DbConnection = "Data Source=localhost;Initial Catalog=test;User Id=test;Password=1234";

        // GET: Board
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                {
                    //List<Board> list = new List<Board>();
                    //list = db.Query<Board>("SELECT * FROM Board").ToList();
                    ViewBag.boardIndex = db.Query<Board>("SELECT * FROM Board ORDER BY Id DESC").ToList();
                }
                //return View(list);
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        /*public JsonResult Index(string Id)
        {
            if (Session["UserId"] != null)
            {
                using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                {
                    //List<Board> list = new List<Board>();
                    //list = db.Query<Board>("SELECT * FROM Board").ToList();
                    return Json(db.Query<CommentList>("sp_SPA_CommentList_Select", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure).ToList());
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }*/

        // GET: Board/Details/
        public ActionResult Details(int Id, string ParentId, string TopId, string Lv, string Comment, string Image)
        {
            if (Session["UserId"] != null)
            {
                using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                {
                    ViewBag.Board = db.Query<Board>("SELECT * FROM Board WHERE Id =" + Id + "ORDER BY Id DESC", new { Id }).SingleOrDefault();
                    //ViewBag.comment = db.Query<CommentList>("SELECT * FROM CommentList (nolock) ORDER BY Id DESC").ToList();
                    ViewBag.comment = db.Query<CommentList>("sp_SPA_CommentList_Select", new { Id = Id, ParentId = ParentId, TopId = TopId, Lv = Lv, Comment = Comment, Image = Image, UserId = Session["UserId"].ToString(), Regdate = DateTime.Now }, null, true, null, System.Data.CommandType.StoredProcedure).ToList();
                    //ViewBag.comment = db.Query<CommentList>("sp_SPA_CommentListWithCTE_Select", new { Id = Id, ParentId = ParentId, TopId = Id, Lv = Lv, UserId = Session["UserId"].ToString(), Comment = Comment, Regdate = DateTime.Now }, null, true, null, System.Data.CommandType.StoredProcedure).ToList();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Board/Details/
        /*public ActionResult Details(CommentList commentlist)
        {
            if (Session["UserId"] != null)
            {
                using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                {
                    //var file = commentlist.Image;
                    string fileName = Path.GetFileName(commentlist.Image.FileName);
                    string filePath = @"https:\\localhost:44329\image\" + fileName;
                    ViewBag.Board = db.Query<Board>("SELECT * FROM Board WHERE Id =" + commentlist.Id + "ORDER BY Id DESC", new { commentlist.Id }).SingleOrDefault();
                    //ViewBag.comment = db.Query<CommentList>("SELECT * FROM CommentList (nolock) ORDER BY Id DESC").ToList();
                    ViewBag.comment = db.Query<CommentList>("sp_SPA_CommentList_Select", new { Id = commentlist.Id, ParentId = commentlist.ParentId, TopId = commentlist.TopId, Lv = commentlist.Lv, Comment = commentlist.Comment, Image = filePath, UserId = Session["UserId"].ToString(), Regdate = DateTime.Now }, null, true, null, System.Data.CommandType.StoredProcedure).ToList();
                    //ViewBag.comment = db.Query<CommentList>("sp_SPA_CommentListWithCTE_Select", new { Id = Id, ParentId = ParentId, TopId = Id, Lv = Lv, UserId = Session["UserId"].ToString(), Comment = Comment, Regdate = DateTime.Now }, null, true, null, System.Data.CommandType.StoredProcedure).ToList();
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }*/

        // GET: Board/Create
        public ActionResult Create()
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

        // POST: Board/Create  
        [HttpPost]
        public ActionResult Create(Board board)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                board.UserId = Session["UserId"].ToString();
                string sql = "INSERT INTO Board (Title, Comment, UserId, Regdate) VALUES (@Title, @Comment, @UserId, GETDATE())";
                db.Execute(sql, board);
            }
            return RedirectToAction("Index");
        }

        // GET: Board/Edit/
        public ActionResult Edit(int Id)
        {
            if (Session["UserId"] != null)
            {
                Board board = new Board();
                if (Session["UserId"].ToString() == board.UserId)
                {
                    //Board board = new Board();
                    using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                    {
                        string sql = "SELECT * FROM Board WHERE Id = @Id";
                        board = db.Query<Board>(sql, new { Id }).SingleOrDefault();
                    }
                    return View(board);
                }
                else
                {
                    return RedirectToAction("MyInfo", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Board/Edit/
        [HttpPost]
        public ActionResult Edit(Board board)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                string sql = "UPDATE Board SET Title = @Title, Comment = @Comment, Regdate = GETDATE() WHERE Id = @Id";
                db.Execute(sql, board);
            }
            return RedirectToAction("Index");
        }

        // GET: Board/Delete/ 
        public ActionResult Delete(int Id, string UserId)
        {
            if (Session["UserId"] != null)
            {
                /*Board board = new Board();
                if (Session["UserId"].ToString() == board.UserId)
                {*/
                Board board = new Board();
                using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                {
                    string sql = "SELECT * FROM Board WHERE Id = @Id";
                    board = db.Query<Board>(sql, new { Id }).SingleOrDefault();
                }
                return View(board);
                /*}
                else
                {
                    return RedirectToAction("Index");
                }*/
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: Board/Delete/
        [HttpPost]
        public ActionResult Delete(Board board)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                string sql = "DELETE FROM Board WHERE Id = @Id";
                db.Execute(sql, board);
            }
            return RedirectToAction("Index");
        }



        /*========================================================================================================*/
        /* 댓글 Reply */
        /*========================================================================================================*/


        /* 댓글 GetReplyList */
        public JsonResult GetReplyList(string Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Select", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }

        /* 댓글 ReplyUpdate */
        /*[HttpPost]
        public JsonResult ReplyUpdate(int Id, string ParentId, string TopId, int Lv, string Comment, string Image)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                if (Id == 0 && ParentId == null) // Insert (신규 저장 완료)
                {
                    //return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentListWithCTE_Insert", new { Id = Id, UserId = Session["UserId"].ToString(), Comment = Comment, Regdate = DateTime.Now }, null, true, null, System.Data.CommandType.StoredProcedure));           
                    object paramss = new { Id = Id, ParentId = ParentId, TopId = TopId, Lv = Lv, UserId = Session["UserId"].ToString(), Comment = Comment, Image = Image };
                    return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Insert", paramss, null, true, null, System.Data.CommandType.StoredProcedure));
                }
                else if (Id != 0 && ParentId != null) // Insert (댓글 등록 완료)
                {
                    return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Insert", new { Id = Id, ParentId = ParentId, TopId = TopId, Lv = Lv, UserId = Session["UserId"].ToString(), Comment = Comment, Image = Image }, null, true, null, System.Data.CommandType.StoredProcedure));
                }
                else // Update (수정 완료)
                {
                    //return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentListWithCTE_Update", new { Id = Id, UserId = Session["UserId"].ToString(), Comment = Comment, Regdate = DateTime.Now }, null, true, null, System.Data.CommandType.StoredProcedure));
                    return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Update", new { Id = Id, Comment = Comment, Image = Image }, null, true, null, System.Data.CommandType.StoredProcedure));
                }
            }
        }*/

        /* 댓글 ReplyUpdate */
        [HttpPost]
        public ActionResult ReplyUpdate(int Id, string ParentId, string TopId, int Lv, string Comment, HttpPostedFileBase file)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                if (file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    file.SaveAs(_path);
                    string filePath = @"https:\\localhost:44329\image\" + fileName;
                    if (Id == 0 && ParentId == null) // Insert (신규 저장 완료)
                    {
                        object paramss = new { Id = Id, ParentId = ParentId, TopId = TopId, Lv = Lv, UserId = Session["UserId"].ToString(), Comment = Comment, Image = filePath, Regdate = DateTime.Now };
                        return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Insert", paramss, null, true, null, System.Data.CommandType.StoredProcedure));
                    }
                    else if (Id != 0 && ParentId != null) // Insert (댓글 등록 완료)
                    {
                        return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Insert", new { Id = Id, ParentId = ParentId, TopId = TopId, Lv = Lv, UserId = Session["UserId"].ToString(), Comment = Comment, Image = filePath, Regdate = DateTime.Now }, null, true, null, System.Data.CommandType.StoredProcedure));
                    }
                    else // Update (수정 완료)
                    {
                        //return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentListWithCTE_Update", new { Id = Id, UserId = Session["UserId"].ToString(), Comment = Comment, Regdate = DateTime.Now }, null, true, null, System.Data.CommandType.StoredProcedure));
                        return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Update", new { Id = Id, Comment = Comment, Image = filePath, Regdate = DateTime.Now }, null, true, null, System.Data.CommandType.StoredProcedure));
                    }
                }
                return View();
            }
        }

        [HttpPost]
        public ActionResult ReplyUpdate2(string Comment, HttpPostedFileBase file)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                string fileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                file.SaveAs(_path);
                string filePath = @"https:\\localhost:44329\image\" + fileName;

                object param2 = new { Comment = Comment, Image = filePath };
                string sql = "INSERT INTO CommentList (Comment, Image) VALUES (" + Comment + "," + filePath + ")";
                return Json(db.Query<Models.SPA.CommentList>(sql, param2));
            }
        }

        [HttpPost]
        public ActionResult UploadFiles(string Comment)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                /*                if (Request.Files.Count > 0)
                                {
                                    try
                                    {*/
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string fname = file.FileName;
                    string fpath = Path.Combine(Server.MapPath("~/UploadedFiles"), fname);
                    file.SaveAs(fpath);
                    string filePath = @"https:\\localhost:44329\image\" + fname;
                    db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_UploadFiles", new { Image = filePath, Comment = Comment }, null, true, null, System.Data.CommandType.StoredProcedure);
                }
                return Json("File Uploaded Successfully!");
                /*                    }
                                    catch (Exception ex)
                                    {
                                        return Json("Error occurred. Error details: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    return Json("No files selected.");
                                }*/
            }


        }

        /* 댓글 ReplyDelete */
        public JsonResult ReplyDelete(int Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                //return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentListWithCTE_Delete", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
                return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Delete", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }

        /* 이미지 파일 업로드 */
        /*[HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/image"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return RedirectToAction("Details");
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return RedirectToAction("Details");
            }
        }*/

        /*[HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/image"), _FileName);
                file.SaveAs(_path);
            }
            return View();
        }*/

        /* 이미지 파일 업로드 */
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var fileName = string.Empty;
            var model = new JsonResultModel();
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                fileName = file.FileName;
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }*/

        /*[HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/image/"), fileName);
                    file.SaveAs(path);
                }
            }
            return RedirectToAction("Details", "Board");
        }*/

        /*public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                try
                {
                    HttpFileCollection hfc = HttpContext.Current.Request.Files;
                    string path = "/image/";
                    for (int i = 0; i < hfc.Count; i++)
                    {
                        HttpPostedFile hpf = hfc[i];
                        if (hpf.ContentLength > 0)
                        {
                            string fileName = "";
                            if (Request.Browser.Browser == "IE")
                            {
                                fileName = Path.GetFileName(hpf.FileName);
                            }
                            else
                            {
                                fileName = hpf.FileName;
                            }
                            string fullPathWithFileName = path + fileName;
                            hpf.SaveAs(Server.MapPath(fullPathWithFileName));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return RedirectToAction("Details", "Board");
        }*/

        /*[HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/image"), fileName);
                file.SaveAs(path);
            }
            return RedirectToAction("Details", "Board");
        }*/
    }
}