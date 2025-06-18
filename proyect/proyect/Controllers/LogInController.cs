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
    }
}