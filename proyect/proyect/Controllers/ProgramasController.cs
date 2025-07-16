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
    public class ProgramasController : Controller
    {
        private VozDelEsteEntities db = new VozDelEsteEntities();

        // GET: Programas
        public ActionResult Index()
        {
            return View(db.Programas.ToList());
        }

        // GET: Programas/Details/5


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programas programas = db.Programas.Find(id);
            if (programas == null)
            {
                return HttpNotFound();
            }
            return View(programas);
        }







       



        // GET: Programas/Create



        [Permiso(NombrePermiso = "Modificar Programas")]
        public ActionResult Create()
        {
            var vm = new ProgramaCompletoViewModel
            {
                ConductoresDisponibles = db.Conductores
                    .Where(c => c.ProgramaId == null)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre
                    }).ToList()
            };

            return View(vm);
        }


        // POST: Programas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso(NombrePermiso = "Modificar Programas")]


        public ActionResult Create(ProgramaCompletoViewModel vm)
        {
            if (HayConflictoHorario(vm.DiaSemana, vm.HoraInicio, vm.HoraFin))
            {
                ModelState.AddModelError("", "Ya existe un programa en ese horario. Elegí otro rango.");
            }

            if (ModelState.IsValid)
            {
                if (vm.ImagenFile != null && vm.ImagenFile.ContentLength > 0)
                {
                    string carpeta = Server.MapPath("~/Content/imagenes/Programas/");
                    if (!Directory.Exists(carpeta))
                        Directory.CreateDirectory(carpeta);

                    string nombreArchivo = Path.GetFileName(vm.ImagenFile.FileName);
                    string rutaCompleta = Path.Combine(carpeta, nombreArchivo);
                    vm.ImagenFile.SaveAs(rutaCompleta);
                    vm.Programa.Imagen = "/Content/imagenes/Programas/" + nombreArchivo;
                }

                db.Programas.Add(vm.Programa);
                db.SaveChanges();

                // Asignar conductor
                if (vm.ConductorIdSeleccionado.HasValue)
                {
                    var conductor = db.Conductores.Find(vm.ConductorIdSeleccionado.Value);
                    if (conductor != null)
                    {
                        conductor.ProgramaId = vm.Programa.Id;
                    }
                }

                // Agregar horario
                db.ProgramacionHoraria.Add(new ProgramacionHoraria
                {
                    ProgramaId = vm.Programa.Id,
                    DiaSemana = vm.DiaSemana,
                    HoraInicio = vm.HoraInicio,
                    HoraFin = vm.HoraFin
                });

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Si hay error, volver a llenar el dropdown
            vm.ConductoresDisponibles = db.Conductores
                .Where(c => c.ProgramaId == null)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                }).ToList();

            return View(vm);
        }

        // GET: Programas/Edit/5
        [Permiso(NombrePermiso = "Modificar Programas")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var programa = db.Programas.Find(id);
            if (programa == null)
                return HttpNotFound();

            // Obtener conductor asignado
            var conductor = db.Conductores.FirstOrDefault(c => c.ProgramaId == programa.Id);

            // Obtener horario actual
            var horario = db.ProgramacionHoraria.FirstOrDefault(h => h.ProgramaId == programa.Id);

            var vm = new ProgramaCompletoViewModel
            {
                Programa = programa,
                ConductorIdSeleccionado = conductor?.Id,
                DiaSemana = horario?.DiaSemana,
                HoraInicio = horario?.HoraInicio ?? TimeSpan.Zero,
                HoraFin = horario?.HoraFin ?? TimeSpan.Zero,
                ProgramacionHorariaId = horario?.Id,
                ConductoresDisponibles = db.Conductores
                    .Where(c => c.ProgramaId == null || c.ProgramaId == programa.Id)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Nombre
                    }).ToList()
            };

            return View(vm);
        }


        // POST: Programas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permiso(NombrePermiso = "Modificar Programas")]

        
        public ActionResult Edit(ProgramaCompletoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var original = db.Programas.Find(vm.Programa.Id);


                if (HayConflictoHorario(vm.DiaSemana, vm.HoraInicio, vm.HoraFin))
                {
                    ModelState.AddModelError("", "Ya existe un programa en ese horario. Elegí otro rango.");
                }
                if (original == null)
                    return HttpNotFound();

                original.Nombre = vm.Programa.Nombre;
                original.Descripcion = vm.Programa.Descripcion;

                // Imagen nueva opcional
                if (vm.ImagenFile != null && vm.ImagenFile.ContentLength > 0)
                {
                    string carpetaDestino = Server.MapPath("~/Content/imagenes/Programas/");
                    if (!Directory.Exists(carpetaDestino))
                        Directory.CreateDirectory(carpetaDestino);

                    string nombreArchivo = Path.GetFileName(vm.ImagenFile.FileName);
                    string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);
                    vm.ImagenFile.SaveAs(rutaCompleta);

                    original.Imagen = "/Content/imagenes/Programas/" + nombreArchivo;
                }

                // Cambiar conductor
                var conductorActual = db.Conductores.FirstOrDefault(c => c.ProgramaId == original.Id);
                if (conductorActual != null)
                    conductorActual.ProgramaId = null;

                if (vm.ConductorIdSeleccionado.HasValue)
                {
                    var nuevoConductor = db.Conductores.Find(vm.ConductorIdSeleccionado.Value);
                    if (nuevoConductor != null)
                        nuevoConductor.ProgramaId = original.Id;
                }

                // Editar horario existente
                if (vm.ProgramacionHorariaId.HasValue)
                {
                    var horario = db.ProgramacionHoraria.Find(vm.ProgramacionHorariaId.Value);
                    if (horario != null)
                    {
                        horario.DiaSemana = vm.DiaSemana;
                        horario.HoraInicio = vm.HoraInicio;
                        horario.HoraFin = vm.HoraFin;
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Si hay error, recargar dropdown
            vm.ConductoresDisponibles = db.Conductores
                .Where(c => c.ProgramaId == null || c.ProgramaId == vm.Programa.Id)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                }).ToList();

            return View(vm);
        }


        // GET: Programas/Delete/5
        [Permiso(NombrePermiso = "Modificar Programas")]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Programas programas = db.Programas.Find(id);
            if (programas == null)
            {
                return HttpNotFound();
            }
            return View(programas);
        }

        // POST: Programas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Permiso(NombrePermiso = "Modificar Programas")]

        
        public ActionResult DeleteConfirmed(int id)
        {
            var programa = db.Programas.Find(id);

            // 🧹 Eliminá horarios relacionados
            var horarios = db.ProgramacionHoraria.Where(h => h.ProgramaId == programa.Id).ToList();
            foreach (var h in horarios)
            {

                db.ProgramacionHoraria.Remove(h);
            }

            db.Programas.Remove(programa);
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


        public ActionResult GrillaHoy()
        {
            string nombreDia = "";

            switch (DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday: nombreDia = "Lunes"; break;
                case DayOfWeek.Tuesday: nombreDia = "Martes"; break;
                case DayOfWeek.Wednesday: nombreDia = "Miércoles"; break;
                case DayOfWeek.Thursday: nombreDia = "Jueves"; break;
                case DayOfWeek.Friday: nombreDia = "Viernes"; break;
                case DayOfWeek.Saturday: nombreDia = "Sábado"; break;
                case DayOfWeek.Sunday: nombreDia = "Domingo"; break;
            }

            var ahora = DateTime.Now.TimeOfDay;

            ViewBag.HoraActual = ahora;

            var programacion = db.ProgramacionHoraria
                .Include(p => p.Programas)
                .Include(p => p.Programas.Conductores)
                .Where(p => p.DiaSemana == nombreDia)
                .OrderBy(p => p.HoraInicio)
                .ToList();

            return View(programacion);
        }

    public ActionResult GrillaSemanal()
    {
        var programacion = db.ProgramacionHoraria
            .Include(p => p.Programas)
            .Include(p => p.Programas.Conductores)
            .ToList();

        return View(programacion);
    }
        private bool HayConflictoHorario(string dia, TimeSpan inicio, TimeSpan fin, int? ignorarProgramaId = null)
        {
            return db.ProgramacionHoraria
                .Any(ph =>
                    ph.DiaSemana == dia &&
                    (ignorarProgramaId == null || ph.ProgramaId != ignorarProgramaId) &&
                    (
                        (inicio >= ph.HoraInicio && inicio < ph.HoraFin) ||  // empieza dentro
                        (fin > ph.HoraInicio && fin <= ph.HoraFin) ||        // termina dentro
                        (inicio <= ph.HoraInicio && fin >= ph.HoraFin)       // lo cubre completo
                    )
                );
        }


    }
}
