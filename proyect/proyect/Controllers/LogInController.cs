using proyect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace proyect.Controllers
{
    public class LogInController : Controller
    {
        private VozDelEsteEntities db = new VozDelEsteEntities();
        public ActionResult SolicitarDatosView(string email, string contrasena)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contrasena))
            {
                ViewBag.ErrorMessage = "Por favor, ingresa tu correo electrónico y contraseña.";
                return View();
            }

            // Buscar al usuario en la base de datos
            Usuarios usuario = db.Usuarios.FirstOrDefault(u => u.Email == email && u.Contrasena == contrasena);

            // Verificar si el usuario es nulo
            if (usuario == null)
            {
                ViewBag.ErrorMessage = "Correo electrónico o contraseña incorrectos.";
                return View();
            }

            // Comprobar el rol del usuario
            if (usuario.Rol == "administrador")
            {
                // Si el rol es "administrador", redirigir al panel de administración
                return RedirectToAction("AdminPanel", "Admin");
            }
            else if (usuario.Rol == "cliente")
            {
                // Si el rol es otro, redirigir a la página de usuario normal
                return RedirectToAction("UserDashboard", "User");
            }
            else
            {
                // Si el rol es otro, redirigir a la página de usuario normal
                return RedirectToAction("UserDashboard", "User");
            }


        }
        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            // Buscar usuario en la base
            var usuario = db.Usuarios
                            .FirstOrDefault(u => u.Email == email && u.Contrasena == password);

            if (usuario != null)
            {
                // Guardar los datos del usuario logueado en sesión
                Session["UsuarioId"] = usuario.Id;
                Session["UsuarioEmail"] = usuario.Email;
                Session["UsuarioRol"] = usuario.Rol;

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return RedirectToAction( "SolicitarDatosView", "LogIn");
        }

        public ActionResult Logout()
        {
            Session.Clear();
                return RedirectToAction("Index", "Home");
        }

    }
}