using GamingForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamingForum.Controllers
{
    public class CommentsController : Controller
    {
        private Models.AppContext db = new Models.AppContext();

        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return Redirect("/Topics/Show/" + comment.TopicId);
        }
        [HttpPost]
        public ActionResult New(Comment comment)
        {
            comment.Date = DateTime.Now;
            try
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Topics/Show/" + comment.TopicId);
            }catch(Exception e)
            {
                return Redirect("/Topics/Show/" + comment.TopicId);
            }
          
        }

        public ActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);
            ViewBag.Comment = comment;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Comment requestComment)
        {
            try
            {
                Comment comment = db.Comments.Find(id);
                if(TryUpdateModel(comment))
                {
                    comment.Content = requestComment.Content;
                    db.SaveChanges();
                }
                return Redirect("/Topics/Show" + comment.TopicId);
            }catch(Exception e)
            {
                return View();
            }
        }




    }
}