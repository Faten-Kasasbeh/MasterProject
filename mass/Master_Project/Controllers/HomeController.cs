using Master_Project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Master_Project.Controllers
{
    public class HomeController : Controller

    {
        public ActionResult Index()
        {
          

            

            Master_Project.Models.Master_projectEntities db = new Master_Project.Models.Master_projectEntities();
                Master_Project.Models.Informaion informaion = new Master_Project.Models.Informaion();
                var info = db.Informaions.ToList();
            foreach (var item in info)
            {

                DateTime date1 = item.LogInDate.Value;
                DateTime date2 = DateTime.Now;
                TimeSpan diff = date2 - date1;
                int result = Convert.ToInt32(diff.TotalDays);
                var information = db.Informaions.Find(item.IdUser);

                if ((result > 30 && item.IsPayed == null && item.Accepted == true))
                {
                    information.IsPayed = false;
                    db.Entry(information).State=System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    //using (var context = new Master_projectEntities())
                    //{
                    //    informaion.IsPayed = false;
                    //    db.SaveChanges();


                    //}
                }
            }
                    
                    return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}