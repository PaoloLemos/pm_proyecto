using proyect.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyect.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [RolAuthorize("administrador")]
        public ActionResult MenuAdmin()
        {
            return View();
        }
    }
}