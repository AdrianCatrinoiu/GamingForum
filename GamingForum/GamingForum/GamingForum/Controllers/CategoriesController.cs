using GamingForum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamingForum.Controllers
{

    static class Globals
    {
        public static int sortare = 0;
    }

    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            if (Globals.sortare == 1)
            {
                var categories = from category in db.Categories
                                 orderby category.CategoryName
                                 select category;
                ViewBag.Categories = categories;
            }
            if (Globals.sortare == 0)
            {
                var categories = from category in db.Categories
                                 orderby category.CategoryId
                                 select category;
                ViewBag.Categories = categories;
            }

            return View();
        }
        public ActionResult Show(int id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.Category = category;
            return View(category);

        }


        //GET - implicit
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New(Category category)
        {
            try
            {
                if (ModelState.IsValid) {

                    db.Categories.Add(category);
                    db.SaveChanges();
                    TempData["message"] = "Categoria a fost adaugata.";
                    return RedirectToAction("Index");
                }
                else { return View(category); }


            }
            catch (Exception e)
            {
                return View(category);
            }

        }


        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.Category = category;
            return View(category);
        }

        [HttpPut]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try 
            {
                Category category = db.Categories.Find(id);
                if (ModelState.IsValid)//delete if it won't work
                {
                    if (TryUpdateModel(category))
                    {
                        category.CategoryName = requestCategory.CategoryName;
                        db.SaveChanges();
                        TempData["message"] = "Categoria a fost modificata.";
                        return RedirectToAction("Index");
                    }
                    return View(requestCategory);

                }
                return View(requestCategory);
            }
            catch (Exception e)
            {
                return View(requestCategory);
            }

        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            TempData["message"] = "Categoria a fost eliminata.";
            return RedirectToAction("Index");
        }
        public ActionResult Reorder(int id)
        {
            Globals.sortare = id;
            TempData["message"] = "Categoria a fost sortata.";

            return RedirectToAction("Index");
        }

    }
}