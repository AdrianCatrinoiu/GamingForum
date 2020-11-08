using GamingForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GamingForum.Controllers
{
    public class TopicsController : Controller
    {
        private Models.AppContext db = new Models.AppContext();
        // GET: Topics
        public ActionResult Index()
        {
            var topics=db.Topics.Include("Category");
            ViewBag.Topics = topics;
            return View();
        }

        public ActionResult Show(int id)
        {
            Topic topic = db.Topics.Find(id);
            ViewBag.Topic = topic;
            ViewBag.Category = topic.Category;
            return View();

        }


        //GET - implicit
        public ActionResult New()
        {
            Topic topic = new Topic();
            topic.Categ = GetAllCategories();
       
            return View(topic);
        }
        [HttpPost]
        public ActionResult New(Topic topic)
        {
            topic.Date = DateTime.Now;
            try
            {
                db.Topics.Add(topic);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost adaugat!";
                return RedirectToAction("Index");
            } catch(Exception e)
            {
                return View();
            }
             
        }
        public ActionResult Edit(int id)
        {
            Topic topic = db.Topics.Find(id);
            ViewBag.Topic = topic;
            ViewBag.Category = topic.Category;
            var categories = from cat in db.Categories select cat;
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Topic requestTopic)
        {
            try
            {
                Topic topic = db.Topics.Find(id);
                if (TryUpdateModel(topic))
                {
                    topic.Title = requestTopic.Title;
                    topic.Content = requestTopic.Content;
                    topic.Date = requestTopic.Date;
                    topic.CategoryId = requestTopic.CategoryId;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // extragem toate categoriile din baza de date
            var categories = from cat in db.Categories
                             select cat;
            // iteram prin categorii
            foreach (var category in categories)
            {
                // adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }


    }
}