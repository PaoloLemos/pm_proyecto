using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using proyect.Models;

namespace proyect.Controllers
{
    public class PatrocinadoresController : Controller
    {
        private VozDelEsteEntities db = new VozDelEsteEntities();

        // GET: Patrocinadores
        public ActionResult Index()
        {
            return View(db.Patrocinadores.ToList());
        }

        // GET: Patrocinadores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patrocinadores patrocinadores = db.Patrocinadores.Find(id);
            if (patrocinadores == null)
            {
                return HttpNotFound();
            }
            return View(patrocinadores);
        }

        // GET: Patrocinadores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patrocinadores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

      
        public ActionResult Create(Patrocinadores patrocinador, HttpPostedFileBase ImagenFile)
        {
            if (ModelState.IsValid)
            {
                // 📦 Guardar imagen si se subió
                if (ImagenFile != null && ImagenFile.ContentLength > 0)
                {
                    string carpeta = Server.MapPath("~/Content/imagenes/Patrocinadores/");
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    // nombre único por si suben con el mismo nombre
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(ImagenFile.FileName);
                    string ruta = Path.Combine(carpeta, nombreArchivo);
                    ImagenFile.SaveAs(ruta);

                    // ✅ ESTA es la línea que guarda la URL relativa en la BD
                    patrocinador.Imagen = "/Content/imagenes/Patrocinadores/" + nombreArchivo;
                }

                // 👉 Asegurate de esto, sin el link nunca va a aparecer
                db.Patrocinadores.Add(patrocinador);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(patrocinador);
        }



        // GET: Patrocinadores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patrocinadores patrocinadores = db.Patrocinadores.Find(id);
            if (patrocinadores == null)
            {
                return HttpNotFound();
            }
            return View(patrocinadores);
        }

        // POST: Patrocinadores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Patrocinadores patrocinadores, HttpPostedFileBase ImagenFile)
        {
            if (ModelState.IsValid)
            {
                var original = db.Patrocinadores.Find(patrocinadores.Id);
                if (original == null)
                    return HttpNotFound();

                original.Nombre = patrocinadores.Nombre;
                original.Descripcion = patrocinadores.Descripcion;
                original.Plan = patrocinadores.Plan;

                if (ImagenFile != null && ImagenFile.ContentLength > 0)
                {
                    string carpeta = Server.MapPath("~/Content/imagenes/Patrocinadores/");
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    string nombreArchivo = Path.GetFileName(ImagenFile.FileName);
                    string ruta = Path.Combine(carpeta, nombreArchivo);
                    ImagenFile.SaveAs(ruta);

                    original.Imagen = "/Content/imagenes/Patrocinadores/" + nombreArchivo;
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patrocinadores);
        }


        // GET: Patrocinadores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patrocinadores patrocinadores = db.Patrocinadores.Find(id);
            if (patrocinadores == null)
            {
                return HttpNotFound();
            }
            return View(patrocinadores);
        }

        // POST: Patrocinadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patrocinadores patrocinadores = db.Patrocinadores.Find(id);
            db.Patrocinadores.Remove(patrocinadores);
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
        public PartialViewResult Logos()
        {
            var patrocinadores = db.Patrocinadores.ToList();
            return PartialView("_PatrocinadoresLogos", patrocinadores);
        }

    }
}
