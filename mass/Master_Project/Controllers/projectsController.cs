using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Master_Project.Models;
using Microsoft.AspNet.Identity;

namespace Master_Project.Controllers
{
    public class projectsController : Controller
    {
        private Master_projectEntities db = new Master_projectEntities();

        // GET: projects
        public ActionResult Index()
        {
            var projects = db.projects.Include(p => p.Informaion);
            var numofProjects=projects.Count();
            ViewBag.numofProjects = numofProjects;

            return View(projects.ToList());

        }


        public ActionResult ProjectsAdmin()
        {
            var projects = db.projects.Include(p => p.Informaion);
            return View(projects.ToList());

        }
        public ActionResult SingleProject(int?id)
        {
           

            var projects = db.projects.Include(p => p.Informaion).Where(p => p.IdProject == id);
            return View(projects.ToList());
        }

        public ActionResult AllProjects()
        {
            var proj = db.projects.Where(I => I.Informaion.Accepted == true && I.Informaion.IsPayed != false).ToList();

            //var projects = db.projects.Include(p => p.Informaion);
            return View(proj.ToList());
        }
        // GET: projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: projects/Create
        public ActionResult Create()
        {
            ViewBag.IdUserProject = new SelectList(db.Informaions, "IdUser", "IdASP");
            return View();
        }

        // POST: projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProject,IdUserProject,ProjectName,ProjectLink,techniques,AboutProject,ProjectType")] project project)
        {
            if (ModelState.IsValid)
            {
                var loginID = User.Identity.GetUserId();
                var idss = db.Informaions.FirstOrDefault(a => a.IdASP == loginID).IdUser;


                project.IdUserProject = idss;
                db.projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUserProject = new SelectList(db.Informaions, "IdUser", "IdASP", project.IdUserProject);
            return View(project);

        }

        // GET: projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUserProject = new SelectList(db.Informaions, "IdUser", "IdASP", project.IdUserProject);
            return View(project);
        }

        // POST: projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProject,IdUserProject,ProjectName,ProjectLink,techniques,AboutProject,ProjectType")] project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUserProject = new SelectList(db.Informaions, "IdUser", "IdASP", project.IdUserProject);
            return View(project);
        }

        // GET: projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

      
        // POST: projects/Delete/5
        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            project project = db.projects.Find(id);
            db.projects.Remove(project);
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
        //delete Projects for Admin
        public ActionResult DeleteProjAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            project project = db.projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [HttpPost, ActionName("DeleteProjAdmin")]

        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedAdmin(int id)
        {
            project project = db.projects.Find(id);
            db.projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Admin", "Informations");
        }
    }
}
