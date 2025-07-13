using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using proyect.Models;
using proyect.Security;

namespace proyect.Controllers
{
    public class UsuariosController : Controller
    {
        private VozDelEsteEntities db = new VozDelEsteEntities();

        // GET: Usuarios
        [Permiso(NombrePermiso = "Ver Clientes")]
        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        // GET: Usuarios/Details/5
        [Permiso(NombrePermiso = "Ver Clientes")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // GET: Usuarios/Create
        [Permiso(NombrePermiso = "Modificar Clientes")]
        public ActionResult Create()
        {
            ViewBag.Roles = db.Roles.ToList();
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso(NombrePermiso = "Modificar Clientes")]

        public ActionResult Create([Bind(Include = "Id,Nombre,Email,Contrasena,RolID")] Usuarios usuarios)
        {
            // Validación de email duplicado
            if (db.Usuarios.Any(u => u.Email == usuarios.Email))
            {
                ModelState.AddModelError("Email", "Este email ya está registrado.");
            }

            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Siempre cargamos los roles si hay error
            ViewBag.Roles = db.Roles.ToList();
            return View(usuarios);
        }


        // GET: Usuarios/Edit/5
        [Permiso(NombrePermiso = "Modificar Clientes")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }

            // Lleno el dropdown de roles
            ViewBag.Roles = db.Roles
                              .Select(r => new { r.RolId, r.Nombre })
                              .ToList();

            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [Permiso(NombrePermiso = "Modificar Clientes")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Email,Contrasena,RolId")] Usuarios usuarios)
        {
            // Validación de email duplicado
            if (db.Usuarios.Any(u => u.Email == usuarios.Email))
            {
                ModelState.AddModelError("Email", "Este email ya está registrado.");
            }

            if (ModelState.IsValid)
            {
                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Roles = db.Roles.Select(r => new { r.RolId, r.Nombre }).ToList();
            return View(usuarios);
        }

        // GET: Usuarios/Delete/5

        [Permiso(NombrePermiso = "Modificar Clientes")]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [ValidateAntiForgeryToken]
        [Permiso(NombrePermiso = "Modificar Clientes")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuarios);
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
