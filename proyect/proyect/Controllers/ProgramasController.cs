﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using proyect.Models;

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
        public ActionResult Create()
        {
            return View();
        }

        // POST: Programas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Imagen,Descripcion")] Programas programas)
        {
            if (ModelState.IsValid)
            {
                db.Programas.Add(programas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(programas);
        }

        // GET: Programas/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Programas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Imagen,Descripcion")] Programas programas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(programas);
        }

        // GET: Programas/Delete/5
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
        public ActionResult DeleteConfirmed(int id)
        {
            Programas programas = db.Programas.Find(id);
            db.Programas.Remove(programas);
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
