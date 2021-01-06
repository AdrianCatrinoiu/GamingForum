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


    public class HomeController : Controller
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

                var topics = db.Topics.Include("Category").Include("User").OrderBy(a => a.Title);
                ViewBag.Topics = topics;

            }
            if (Globals.sortare == 0)
            {
                var categories = from category in db.Categories
                                 orderby category.CategoryId
                                 select category;
                ViewBag.Categories = categories;

                var topics = db.Topics.Include("Category").Include("User").OrderBy(a => a.Title);
                ViewBag.Topics = topics;
            }

            return View();
        }


        public ActionResult Reorder(int id)
        {
            Globals.sortare = id;
            TempData["message"] = "Categoria a fost sortata.";

            return RedirectToAction("Index");
        }

    }
}