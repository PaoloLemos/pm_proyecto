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
using proyect.Security;

namespace proyect.Controllers
{
    public class NoticiasController : Controller
    {
        private VozDelEsteEntities db = new VozDelEsteEntities();

        // GET: Noticias

        [AllowAnonymous]

        public ActionResult Index()
        {
            return View(db.Noticias.ToList());
        }

        // GET: Noticias/Details/5
        [AllowAnonymous]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticias noticias = db.Noticias.Find(id);
            if (noticias == null)
            {
                return HttpNotFound();
            }
            return View(noticias);
        }

        // GET: Noticias/Create



        [Permiso(NombrePermiso = "Modificar Noticias")]

        public ActionResult Create()
        {
            return View();
        }

        // POST: Noticias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso(NombrePermiso = "Modificar Noticias")]

       
        public ActionResult Create(Noticias noticia)
        {
            if (ModelState.IsValid)
            {
                if (noticia.ImagenFile != null && noticia.ImagenFile.ContentLength > 0)
                {
                    string carpetaDestino = Server.MapPath("~/Content/imagenes/Noticias/");
                    if (!Directory.Exists(carpetaDestino))
                    {
                        Directory.CreateDirectory(carpetaDestino); // Asegura que exista la carpeta
                    }

                    string nombreArchivo = Path.GetFileName(noticia.ImagenFile.FileName);
                    string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);
                    noticia.ImagenFile.SaveAs(rutaCompleta);

                    // Guarda solo la ruta relativa para usar en src
                    noticia.Imagen = "/Content/imagenes/Noticias/" + nombreArchivo;
                }

                noticia.FechaPublicacion = DateTime.Now;
                db.Noticias.Add(noticia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(noticia);
        }



        // GET: Noticias/Edit/5

        [Permiso(NombrePermiso = "Modificar Noticias")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticias noticias = db.Noticias.Find(id);
            if (noticias == null)
            {
                return HttpNotFound();
            }
            return View(noticias);
        }

        // POST: Noticias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso(NombrePermiso = "Modificar Noticias")]

        
        public ActionResult Edit(int id, Noticias noticia)
        {
            if (ModelState.IsValid)
            {
                var original = db.Noticias.Find(id);
                if (original == null) return HttpNotFound();

                original.Titulo = noticia.Titulo;
                original.Contenido = noticia.Contenido;

                if (noticia.ImagenFile != null && noticia.ImagenFile.ContentLength > 0)
                {
                    string carpetaDestino = Server.MapPath("~/Content/imagenes/Noticias/");
                    if (!Directory.Exists(carpetaDestino))
                    {
                        Directory.CreateDirectory(carpetaDestino);
                    }

                    string nombreArchivo = Path.GetFileName(noticia.ImagenFile.FileName);
                    string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);
                    noticia.ImagenFile.SaveAs(rutaCompleta);

                    original.Imagen = "/Content/imagenes/Noticias/" + nombreArchivo;
                }
                // Si no se sube nueva imagen, se mantiene la original.Imagen

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            noticia.Id = id;
            return View(noticia);
        }


        // GET: Noticias/Delete/5

        [Permiso(NombrePermiso = "Modificar Noticias")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Noticias noticias = db.Noticias.Find(id);
            if (noticias == null)
            {
                return HttpNotFound();
            }
            return View(noticias);
        }

        // POST: Noticias/Delete/5
        [Permiso(NombrePermiso = "Modificar Noticias")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Noticias noticias = db.Noticias.Find(id);
            db.Noticias.Remove(noticias);
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
