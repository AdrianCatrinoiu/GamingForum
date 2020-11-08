using GamingForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GamingForum.Controllers
{
    public class ArticlesController : Controller
    {
        private Models.AppContext db = new Models.AppContext();
        // GET: Articles
        public ActionResult Index()
        {
            var articles=db.Articles.Include("Category");
            ViewBag.Articles = articles;
            return View();
        }

        public ActionResult Show(int id)
        {
            Article article = db.Articles.Find(id);
            ViewBag.Article = article;
            ViewBag.Category = article.Category;
            return View();

        }


        //GET - implicit
        public ActionResult New()
        {
            Article article = new Article();
            article.Categ = GetAllCategories();
       
            return View(article);
        }
        [HttpPost]
        public ActionResult New(Article article)
        {
            article.Date = DateTime.Now;
            try
            {
                db.Articles.Add(article);
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
            Article article = db.Articles.Find(id);
            ViewBag.Article = article;
            ViewBag.Category = article.Category;
            var categories = from cat in db.Categories select cat;
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Article requestArticle)
        {
            try
            {
                Article article = db.Articles.Find(id);
                if (TryUpdateModel(article))
                {
                    article.Title = requestArticle.Title;
                    article.Content = requestArticle.Content;
                    article.Date = requestArticle.Date;
                    article.CategoryId = requestArticle.CategoryId;
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
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
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