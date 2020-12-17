using GamingForum.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GamingForum.Controllers
{

    public class TopicsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Topics
        //[Authorize(Roles ="User,Editor,Admin")]
        public ActionResult Index()
        {
            var topics=db.Topics.Include("Category").Include("User");
            ViewBag.Topics = topics;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            ViewBag.esteAdmin = User.IsInRole("Admin");

            if (User.IsInRole("Editor") || User.IsInRole("Admin") || User.IsInRole("User"))
            {
                ViewBag.afisareButoane = true;
            }

            return View();
        }
        //[Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Show(int id)
        {
            Topic topic = db.Topics.Find(id);

            ViewBag.afisareButoane = false;
            if(User.IsInRole("Editor")||User.IsInRole("Admin")|| topic.UserId == User.Identity.GetUserId())
            {
                ViewBag.afisareButoane = true;
            }
            ViewBag.esteUser = User.IsInRole("User");
            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            ViewBag.Topic = topic;
            ViewBag.Category = topic.Category;
            return View(topic);

        }


        //GET - implicit
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New()
        {
            Topic topic = new Topic();
            topic.Categ = GetAllCategories();
            topic.UserId = User.Identity.GetUserId();
            return View(topic);
        }
        [HttpPost]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New(Topic topic)
        {
            topic.Categ = GetAllCategories();
            topic.Date = DateTime.Now;
            topic.UserId = User.Identity.GetUserId();

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

        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id)
        {
            Topic topic = db.Topics.Find(id);
            topic.Categ = GetAllCategories();
            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.esteModerator = User.IsInRole("Editor");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            if (topic.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(topic);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul de a modifica asupra unui topic care nu va apartine";
                return RedirectToAction("Index");
            }
            
        }

        [HttpPut]
        [Authorize(Roles = "User,Editor,Admin")]
        public ActionResult Edit(int id, Topic requestTopic)
        {
            requestTopic.Categ = GetAllCategories();
            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.esteModerator = User.IsInRole("Editor");
            ViewBag.utilizatorCurent = User.Identity.GetUserId();

            try
            {
                if(ModelState.IsValid)
                {
                    Topic topic = db.Topics.Find(id);
                    if (topic.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
                    {
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
                        TempData["message"] = "Nu aveti dreptul de a modifica asupra unui topic care nu va apartine";
                        return RedirectToAction("Index");
                    }
                        
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
        [Authorize(Roles = "Editor,Admin")]
        public ActionResult Delete(int id)
        {
            Topic topic = db.Topics.Find(id);
            ViewBag.esteAdmin = User.IsInRole("Admin");
            ViewBag.esteModerator = User.IsInRole("Editor");
            if (topic.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                db.Topics.Remove(topic);
                db.SaveChanges();
                TempData["message"] = "Topic-ul a fost sters!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul de a modifica asupra unui topic care nu va apartine";
                return RedirectToAction("Index");
            }
                
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