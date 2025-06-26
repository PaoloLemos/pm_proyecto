using proyect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace proyect.Controllers
{
    
        public class LogInController : Controller
        {
            private VozDelEsteEntities db = new VozDelEsteEntities();

            // GET: muestra el form de login
            [AllowAnonymous]
            [HttpGet]
            public ActionResult SolicitarDatosView()
            {
                return View();
            }

            // POST: procesa el login
            [AllowAnonymous]
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Index(string email, string password)
            {
                // 1. Validar que envíen algo
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    ViewBag.Error = "Por favor ingresa correo y contraseña.";
                    return View("SolicitarDatosView");
                }

                // 2. Buscar por email
                var usuario = db.Usuarios.FirstOrDefault(u => u.Email == email);
                if (usuario != null && Crypto.VerifyHashedPassword(usuario.Contrasena, password))
                {
                    // 3. Login exitoso: guardo sesión y redirijo
                    Session["UsuarioId"] = usuario.Id;
                    Session["UsuarioEmail"] = usuario.Email;
                    Session["UsuarioLogueado"] = true;

                var permisos = db.RolesPermisos
                        .Where(rp => rp.RolId == usuario.RolID)
                        .Select(rp => rp.Permisos.Nombre)
                        .ToList();
                string userData = string.Join(",", permisos);
                var ticket = new FormsAuthenticationTicket(
                    version: 1,
                    name: usuario.Nombre,
                    issueDate: DateTime.Now,
                    expiration: DateTime.Now.AddHours(8),
                    isPersistent: false,
                    userData: userData
                );

                string encryptedTicket = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(
                    FormsAuthentication.FormsCookieName,
                    encryptedTicket
                );
                Response.Cookies.Add(cookie);

                return RedirectToAction("Index", "Home");
                }

                // 4. Si falla: muestro error en la misma vista
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View("SolicitarDatosView");
            }

        // Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Borra la sesión y la cookie de FormsAuth
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

    }
}
