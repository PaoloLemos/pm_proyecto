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
    public class ConductoresController : Controller
    {
        private VozDelEsteEntities db = new VozDelEsteEntities();

        // GET: Conductores
        public ActionResult Index()
        {
            var conductores = db.Conductores.Include(c => c.Programas);

            return View(conductores.ToList());
        }

        // GET: Conductores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conductores conductores = db.Conductores.Find(id);
            if (conductores == null)
            {
                return HttpNotFound();
            }
            return View(conductores);
        }

        // GET: Conductores/Create
        public ActionResult Create()
        {
            ViewBag.ProgramaId = new SelectList(db.Programas, "Id", "Nombre");
            return View();
        }

        // POST: Conductores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public ActionResult Create(Conductores conductor, HttpPostedFileBase FotoFile)
        {
            if (ModelState.IsValid)
            {
                // SUBIR IMAGEN
                if (FotoFile != null && FotoFile.ContentLength > 0)
                {
                    string carpeta = Server.MapPath("~/Content/imagenes/Conductores/");
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    string nombreArchivo = Path.GetFileName(FotoFile.FileName);
                    string ruta = Path.Combine(carpeta, nombreArchivo);
                    FotoFile.SaveAs(ruta);

                    conductor.foto = "/Content/imagenes/Conductores/" + nombreArchivo;
                }

                db.Conductores.Add(conductor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(conductor);
        }

        // GET: Conductores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conductores conductores = db.Conductores.Find(id);
            if (conductores == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProgramaId = new SelectList(db.Programas, "Id", "Nombre", conductores.ProgramaId);
            return View(conductores);
        }

        // POST: Conductores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProgramaId,Nombre,Bio")] Conductores conductores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conductores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProgramaId = new SelectList(db.Programas, "Id", "Nombre", conductores.ProgramaId);
            return View(conductores);
        }

        // GET: Conductores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conductores conductores = db.Conductores.Find(id);
            if (conductores == null)
            {
                return HttpNotFound();
            }
            return View(conductores);
        }

        // POST: Conductores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conductores conductores = db.Conductores.Find(id);
            db.Conductores.Remove(conductores);
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
