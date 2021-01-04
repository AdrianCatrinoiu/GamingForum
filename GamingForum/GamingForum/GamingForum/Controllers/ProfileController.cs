using GamingForum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamingForum.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profile
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == user);
            var hasProfile = false;
            //luam profilul
            int pid = 0;
            var about = "";
            var profiles = from p in db.Profiles
                           select p;
            foreach (var p in profiles)
            {

                if  (p.User != null && (p.User.Id == currentUser.Id))
                {
                    hasProfile = true;
                    pid = p.Id;
                    about = p.About;
                }
            }
            if (pid!=0)
            {
            ViewBag.pid = pid;
            }

            ViewBag.About = about;
            ViewBag.hasProfile = hasProfile;
            ViewBag.userId = currentUser.Id;
            ViewBag.username = currentUser.UserName;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            System.Diagnostics.Debug.WriteLine("aici index");

            return View();
        }


        // GET:
        //[Authorize(Roles = "User,Editor,Admin")]

        public ActionResult New()
        {
            Profile profile = new Profile();
           

            return View(profile);
        }
        [HttpPost]
        //[Authorize(Roles = "User,Editor,Admin")]
        public ActionResult New(Profile profile)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    byte[] pic = null;

                    if (Request.Files.Count>0)
                    {

                        HttpPostedFileBase imgFile = Request.Files["ProfilePicture"];
                        using (var binary = new BinaryReader(imgFile.InputStream))
                        {
                            pic = binary.ReadBytes(imgFile.ContentLength);
                        }

                    }



                    profile.Image = pic;
                    var user = User.Identity.GetUserId();
                    ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == user);

                    profile.User=currentUser;

                    db.Profiles.Add(profile);
                    db.SaveChanges();
                    TempData["message"] = "Profilul a fost creat!";
                    return RedirectToAction("Index");
                }

                return View(profile);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

        }
        public ActionResult Edit(string id)
        {
            var user = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == user);
            int pid = 0;
            var profiles = from p in db.Profiles
                           select p;
            foreach (var p in profiles)
            {

                if (p != null && p.User != null && (p.User.Id == currentUser.Id))
                {
                    pid = p.Id;
                }
            }

            Profile profile = db.Profiles.Find(pid);
            System.Diagnostics.Debug.WriteLine("aici0 "+profile.Id);

            return View(profile);
        } 
        [HttpPost]
        public ActionResult Edit(int id,Profile requestProfile)
        {
            System.Diagnostics.Debug.WriteLine("aici1 "+id);

            try
            {

                if (ModelState.IsValid)
                {
                    
                    Profile profile = db.Profiles.Find(id);

                    if (TryUpdateModel(profile))
                    {
                        


                        profile.About = requestProfile.About;

                        var pic = profile.Image;

                        

                        if (Request.Files.Count > 0&& Request.Files[0].ContentLength>0)
                            {
                                HttpPostedFileBase imgFile = Request.Files["ProfilePicture"];
                                using (var binary = new BinaryReader(imgFile.InputStream))
                                {
                                    pic = binary.ReadBytes(imgFile.ContentLength);
                                }
                            System.Diagnostics.Debug.WriteLine("aici 1" + pic);

                        }
                        profile.Image = pic;
                        TempData["message"] = "Profilul a fost modificat!";

                        db.SaveChanges();

                    }
                    return RedirectToAction("Index");


                }
                else {                    return View(requestProfile); }
            }
            catch(Exception e)
            {
                return View(requestProfile);
            }
        }

        public FileContentResult ProfilePicture()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                if (userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);
                    return File(imageData, "image/png");
                }

                var bdProfile = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var profileImage = bdProfile.Profiles.Where(x => x.User.Id == userId).FirstOrDefault();
                return new FileContentResult(profileImage.Image, "image/jpeg");
            }        
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");
                System.Diagnostics.Debug.WriteLine("aici 5");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }

            
        }

    }
}