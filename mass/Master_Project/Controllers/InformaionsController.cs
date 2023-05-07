using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Master_Project.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace Master_Project.Controllers
{
    public class InformaionsController : Controller
    {
        private Master_projectEntities db = new Master_projectEntities();

        // GET: Informaions
        public ActionResult Index()
        {
            var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1);
         

            return View(informaions.ToList());
        }

        [Authorize(Roles = "Admin")]

        public ActionResult Admin()
        {
            var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1);
            var numofuser = db.Informaions.Count();
            var numOfPayed = db.Informaions.Where(i => i.IsPayed == true).Count();
            var NumOfAccept = db.Informaions.Where(i => i.Accepted == true).Count();
            ViewBag.NumOfAccept=NumOfAccept;
            ViewBag.numOfPayed = numOfPayed;
            ViewBag.NumOfUser = numofuser;
            return View(informaions.ToList());
        }

        [Authorize(Roles = "Admin")]

        public ActionResult Requests()
        {
            var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1).Where(i=>i.Accepted==null && i.IsPayed==null);

            return View(informaions.ToList());
        }


        [Authorize(Roles = "Admin")]

        public ActionResult UsersAdmin(int?id)
        {
            if (id == null)
            {
                var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1);
                return View(informaions.ToList());

            }
            if ( id==1){
                var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1).Where(i=>i.Accepted==false);
                return View(informaions.ToList());

            }
            if (id == 2)
            {
                var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1).Where(i=>i.Accepted==true);
                return View(informaions.ToList());

            }


            return View("index");
        }

        //accepted
        [Authorize(Roles = "Admin")]

        public ActionResult Accepted(int id)
{
    using (var context = new Master_projectEntities())
    {
        var information = db.Informaions.Find(id);

        if (information != null)
        {
            information.Accepted = true;
           
            db.SaveChanges();
        }
    }

    return RedirectToAction("Requests");
}


        //rejected
        public ActionResult Rejected(int id)
        {
            using (var context = new Master_projectEntities())
            {
                var information = db.Informaions.Find(id);

                    information.Accepted = false;
                    db.SaveChanges();
                     }

            return RedirectToAction("Requests");
        }

        public ActionResult Users(int?id)
        {
            if(id == null)
            {
                var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1).Where(i => i.Accepted == true && i.IsPayed != false);
                return View(informaions.ToList());

            }
            if (id == 1)
            {
                var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1).Where(i=>i.JobTitle==1 && i.Accepted==true && i.IsPayed != false);
                return View(informaions.ToList());
            }
            if(id == 2){
                var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1).Where(i => i.JobTitle == 2 && i.Accepted == true && i.IsPayed != false);
                return View(informaions.ToList());
            }
            if (id == 3)
            {
                var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1).Where(i => i.JobTitle == 3 && i.Accepted == true && i.IsPayed != false);
                return View(informaions.ToList());
            }
            return View("index");
        }

        //public void CheckifPayed()
        //{
        //    var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1);
        //   foreach(var x in informaions)
        //    {
        //        DateTime date1 = x.LogInDate.Value;
        //        DateTime date2 = DateTime.Now;
        //        TimeSpan diff = date2 - date1;
        //        int result = Convert.ToInt32(diff.TotalDays);
        //        if ((result > 30 && x.IsPayed == null && x.Accepted == true))
        //        {

        //            using (var context = new Master_projectEntities())
        //            {

        //                x.IsPayed = false;
        //                db.SaveChanges();

        //            }

        //        }
        //    }
        //}

        [Authorize]

        public ActionResult Portfolio()
        {
            var loginID = User.Identity.GetUserId();
            var idss = db.Informaions.FirstOrDefault(a => a.IdASP == loginID).IdUser;

            var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1).Where(i=>i.IdASP==loginID);
            DateTime date1 = informaions.FirstOrDefault(i => i.IdUser == idss).LogInDate.Value;
            DateTime date2 = DateTime.Now;
            TimeSpan diff = date2 - date1;
            int result = Convert.ToInt32(diff.TotalDays);
            var information = db.Informaions.Find(idss);
            int expireTime = 30 - result;
            Session["days12"] = expireTime;
            if ((result > 30 && information.IsPayed != true && information.Accepted==true)) {
                
             

                return RedirectToAction("PayView", new { id= idss });
            }
            return View(informaions.ToList());
        }

         
        public ActionResult Profile1(int?id)
        {

            var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1).Where(i => i.IdUser == id);
            return View(informaions.ToList());
        }


        //Pay Viwe
        public ActionResult PayView(int? id)
        {

            var informaions = db.Informaions.Include(i => i.AspNetUser).Include(i => i.City).Include(i => i.Job).Include(i => i.University1).Where(i => i.IdUser == id);
            return View(informaions.ToList());
        }


        //Pay 
        public ActionResult Pay()
        {
            var loginID = User.Identity.GetUserId();
            var idss = db.Informaions.FirstOrDefault(a => a.IdASP == loginID).IdUser;
            using (var context = new Master_projectEntities())
            {
                var information = db.Informaions.Find(idss);

                information.IsPayed = true;
                information.Accepted= true;
                db.SaveChanges();
            }

            return RedirectToAction("Portfolio");
        }
        // GET: Informaions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Informaion informaion = db.Informaions.Find(id);
            if (informaion == null)
            {
                return HttpNotFound();
            }
            return View(informaion);
        }

        // GET: Informaions/Create
        public ActionResult Create()
        {
            ViewBag.IdASP = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.CityInfo = new SelectList(db.Cities, "IdCity", "CityName");
            ViewBag.JobTitle = new SelectList(db.Jobs, "IdJob", "JobName");
            ViewBag.University = new SelectList(db.Universities, "IdUnivesrity", "UniversityName");
            return View();
        }

        // POST: Informaions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUser,IdASP,FirstName,LastName,Gender,Ntionality,BirthDate,JobTitle,Img,CV,University,Major,GradDate,About,Skills,CityInfo,Github,Phone,Experiences,LogInDate,Accepted,IsPayed")] Informaion informaion, HttpPostedFileBase Img, HttpPostedFileBase CV)
        {
            if (ModelState.IsValid)
            {
                if (Img != null)
                {
                    if (!Img.ContentType.ToLower().StartsWith("image/"))
                    {
                        ModelState.AddModelError("", "file uploaded is not an image");
                        return View(informaion);
                    }
                    string folderPath = Server.MapPath("~/Content/Images");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string fileName = Path.GetFileName(Img.FileName);
                    string path = Path.Combine(folderPath, fileName);
                    Img.SaveAs(path);
                    informaion.Img = "../Content/Images/" + fileName;
                }
                else
                {
                    ViewBag.CityInfo = new SelectList(db.Cities, "IdCity", "CityName", informaion.CityInfo);
                    ViewBag.JobTitle = new SelectList(db.Jobs, "IdJob", "JobName", informaion.JobTitle);
                    ViewBag.University = new SelectList(db.Universities, "IdUnivesrity", "UniversityName", informaion.University);
                    ModelState.AddModelError("", "Please upload an image.");
                    return View(informaion);
                }
                //---------------------------------------------
                if (CV != null)
                {
                    if (!CV.ContentType.ToLower().StartsWith("application/pdf"))
                    {
                        ModelState.AddModelError("", "File uploaded is not a PDF.");
                        return View(informaion);
                    }

                    string folderPath = Server.MapPath("~/Content/CV");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    string fileName = Path.GetFileName(CV.FileName);
                    string path = Path.Combine(folderPath, fileName);
                    CV.SaveAs(path);
                    informaion.CV = "../Content/CV/" + fileName;
                }
                else
                {
                    ViewBag.CityInfo = new SelectList(db.Cities, "IdCity", "CityName", informaion.CityInfo);
                    ViewBag.JobTitle = new SelectList(db.Jobs, "IdJob", "JobName", informaion.JobTitle);
                    ViewBag.University = new SelectList(db.Universities, "IdUnivesrity", "UniversityName", informaion.University);
                    ModelState.AddModelError("", "Please upload a CV.");
                    return View(informaion);

                }
                var loginID = User.Identity.GetUserId();
                informaion.IdASP = loginID;
                informaion.IsPayed = null;
                informaion.Accepted = null;
            
                informaion.LogInDate= DateTime.Now;
                db.Informaions.Add(informaion);
                db.SaveChanges();
                return RedirectToAction("Portfolio", "Informaions");
            }

            //ViewBag.IdASP = new SelectList(db.AspNetUsers, "Id", "Email", informaion.IdASP);
            ViewBag.CityInfo = new SelectList(db.Cities, "IdCity", "CityName", informaion.CityInfo);
            ViewBag.JobTitle = new SelectList(db.Jobs, "IdJob", "JobName", informaion.JobTitle);
            ViewBag.University = new SelectList(db.Universities, "IdUnivesrity", "UniversityName", informaion.University);
            return View(informaion);
        }

        // GET: Informaions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Informaion informaion = db.Informaions.Find(id);
            if (informaion == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdASP = new SelectList(db.AspNetUsers, "Id", "Email", informaion.IdASP);
            ViewBag.CityInfo = new SelectList(db.Cities, "IdCity", "CityName", informaion.CityInfo);
            ViewBag.JobTitle = new SelectList(db.Jobs, "IdJob", "JobName", informaion.JobTitle);
            ViewBag.University = new SelectList(db.Universities, "IdUnivesrity", "UniversityName", informaion.University);
            return View(informaion);
        }

        // POST: Informaions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUser,IdASP,FirstName,LastName,Gender,Ntionality,BirthDate,JobTitle,Img,CV,University,Major,GradDate,About,Skills,CityInfo,Github,Phone,Experiences,LogInDate,Accepted,IsPayed")] Informaion informaion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(informaion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdASP = new SelectList(db.AspNetUsers, "Id", "Email", informaion.IdASP);
            ViewBag.CityInfo = new SelectList(db.Cities, "IdCity", "CityName", informaion.CityInfo);
            ViewBag.JobTitle = new SelectList(db.Jobs, "IdJob", "JobName", informaion.JobTitle);
            ViewBag.University = new SelectList(db.Universities, "IdUnivesrity", "UniversityName", informaion.University);
            return View(informaion);
        }

        // GET: Informaions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Informaion informaion = db.Informaions.Find(id);
            if (informaion == null)
            {
                return HttpNotFound();
            }
            return View(informaion);
        }

        // POST: Informaions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Informaion informaion = db.Informaions.Find(id);
            db.Informaions.Remove(informaion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
