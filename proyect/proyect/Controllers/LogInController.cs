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
                    Session["UsuarioRol"] = usuario.Rol.ToLower();
                    return RedirectToAction("Index", "Home");
                }

                // 4. Si falla: muestro error en la misma vista
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View("SolicitarDatosView");
            }

            // Logout
            public ActionResult Logout()
            {
                Session.Clear();
                return RedirectToAction("Index", "Home");
            }
        }
    }
