using Dapper;
using RegisterAndLogin6.Models.SPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegisterAndLogin6.Controllers
{
    public class SPAController : Controller
    {
        /* DB 연결 */
        public string DbConnection = "Data Source=localhost;Initial Catalog=test;User Id=test;Password=1234";

        // GET: SPA
        /*public JsonResult Index(string Id)
        {
            if (Session["UserId"] != null)
            {
                using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                {
                    return Json(db.Query<Models.SPA.Comment>("sp_SPA_Comment", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
                }
            }
            else
            {
                return ;
            }
        }*/

        // GET: SPA
        /*public JsonResult Index(string Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                return Json(db.Query<Models.SPA.CommmentList>("sp_SPA_CommentList_Select", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }*/

        // GET: SPA
        public ActionResult Index(string Id)
        {
            if (Session["UserId"] != null)
            {
                using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                {
                    ViewBag.CommentList = db.Query<CommentList>("SELECT * FROM CommentList (nolock)").ToList();
                    //ViewBag.CommentList = db.Query<CommentList>("sp_SPA_CommentList_Select", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure).ToList();
                    //return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Select", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public ActionResult Update(int Id, string Comment)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                string sql = string.Empty;
                if (Id == 0) // Insert
                {
                    sql = string.Format("INSERT INTO CommentList (Comment, UserId, Regdate) values ('{0}', '{1}', GETDATE())", Comment, Session["UserId"].ToString());
                }
                else // Update
                {
                    sql = string.Format("UPDATE CommentList SET Comment = '{0}' WHERE Id = {1}", Comment, Id);
                }

                db.Execute(sql);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                string sql = string.Format("DELETE FROM CommentList WHERE Id = {0}", Id);
                db.Execute(sql);
            }
            return RedirectToAction("Index");
        }


        //------------------------------------------------------------------------------
        // Comment: 게시글 //
        //------------------------------------------------------------------------------

        /* 게시글 Read */
        /*public JsonResult CommentList(string Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Select", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }*/

        /* 게시글 Create */
        /*public JsonResult CommentSubmit(string Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Insert", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }

        *//* 게시글 Update *//*
        public JsonResult CommentUpdate(string Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Update", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }

        *//* 게시글 Delete *//*
        public JsonResult CommentDelete(string Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                return Json(db.Query<Models.SPA.CommentList>("sp_SPA_CommentList_Delete", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }




        //------------------------------------------------------------------------------
        // Reply: 댓글 //
        //------------------------------------------------------------------------------

        *//* 댓글 Read *//*
        public JsonResult ReplyList(string Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                return Json(db.Query<Models.SPA.ReplyList>("sp_SPA_ReplyList_Select", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }

        *//* 댓글 Create *//*
        public JsonResult ReplySubmit(string Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                return Json(db.Query<Models.SPA.ReplyList>("sp_SPA_ReplyList_Insert", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }

        *//* 댓글 Update *//*
        public JsonResult ReplyUpdate(string Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                return Json(db.Query<Models.SPA.ReplyList>("sp_SPA_ReplyList_Update", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }

        *//* 댓글 Delete *//*
        public JsonResult ReplyDelete(string Id)
        {
            using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
            {
                return Json(db.Query<Models.SPA.ReplyList>("sp_SPA_ReplyList_Delete", new { Id = Id }, null, true, null, System.Data.CommandType.StoredProcedure));
            }
        }*/
    }
}