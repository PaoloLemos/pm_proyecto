using proyect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyect.Controllers
{
    public class HomeController : Controller
    {


        private VozDelEsteEntities db = new VozDelEsteEntities();
        [AllowAnonymous]
        public ActionResult Index()
        {


            var programasRecientes = db.Programas
          .OrderByDescending(p => p.Id)
          .Take(3)
          .ToList();


            return View(programasRecientes);
        }
        [AllowAnonymous]

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AllowAnonymous]

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}