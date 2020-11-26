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
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            return View();
        }

        public ActionResult Show(int id)
        {
            Topic topic = db.Topics.Find(id);
            ViewBag.Topic = topic;
            ViewBag.Category = topic.Category;
            return View(topic);

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
            topic.Categ = GetAllCategories();
            topic.Date = DateTime.Now;
            try
            {
                if(ModelState.IsValid)
                {
                    db.Topics.Add(topic);
                    db.SaveChanges();
                    TempData["message"] = "Topic-ul a fost adaugat!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(topic);
                }
                
            } catch(Exception e)
            {
                return View(topic);
            }
             
        }
        public ActionResult Edit(int id)
        {
            Topic topic = db.Topics.Find(id);
            topic.Categ = GetAllCategories();
            
            return View(topic);
        }

        [HttpPut]
        public ActionResult Edit(int id, Topic requestTopic)
        {
            requestTopic.Categ = GetAllCategories();
            try
            {
                if(ModelState.IsValid)
                {
                    Topic topic = db.Topics.Find(id);
                    if (TryUpdateModel(topic))
                    {

                        topic.Title = requestTopic.Title;
                        topic.Content = requestTopic.Content;
                        topic.CategoryId = requestTopic.CategoryId;
                        db.SaveChanges();
                        TempData["message"] = "Topic-ul a fost modificat!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(requestTopic);
                }
            }
            catch (Exception e)
            {
                return View(requestTopic);
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Topic topic = db.Topics.Find(id);
            db.Topics.Remove(topic);
            db.SaveChanges();
            TempData["message"] = "Topic-ul a fost sters!";
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