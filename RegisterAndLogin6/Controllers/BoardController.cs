using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;
using RegisterAndLogin6.Models;

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
                List<Board> list = new List<Board>();
                using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                {
                    list = db.Query<Board>("SELECT * FROM Board").ToList();
                }
                return View(list);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Board/Details/
        public ActionResult Details(int Id)
        {
            if (Session["UserId"] != null)
            {
                Board board = new Board();
                using (var db = new System.Data.SqlClient.SqlConnection(DbConnection))
                {
                    board = db.Query<Board>("SELECT * FROM Board WHERE Id =" + Id, new { Id }).SingleOrDefault();
                }
                return View(board);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

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
                /*if (Session["UserId"].ToString() == board.UserId)
                {*/
                //Board board = new Board();
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
    }
}