using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ListaReproduccionMVC.Models;

namespace ListaReproduccionMVC.Controllers
{
    public class CancionesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private int? ObtenerUsuarioId()
        {
            if (Session["UsuarioId"] == null)
            {
                return null;
            }

            return (int)Session["UsuarioId"];
        }

        // GET: Canciones
        public ActionResult Index()
        {
            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            var canciones = db.Canciones
                .Where(c => c.UsuarioId == usuarioId.Value)
                .ToList();

            return View(canciones);
        }

        // GET: Canciones/Details/5
        public ActionResult Details(int? id)
        {
            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cancion cancion = db.Canciones
                .FirstOrDefault(c => c.CancionId == id.Value && c.UsuarioId == usuarioId.Value);

            if (cancion == null)
            {
                return HttpNotFound();
            }

            return View(cancion);
        }

        // GET: Canciones/Create
        public ActionResult Create()
        {
            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            return View();
        }

        // POST: Canciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CancionId,Titulo,Artista,Genero,EstadoAnimo,ObjetivoEstudio,Duracion,Enlace")] Cancion cancion)
        {
            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            cancion.UsuarioId = usuarioId.Value;
            ModelState.Remove("UsuarioId");

            if (ModelState.IsValid)
            {
                db.Canciones.Add(cancion);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(cancion);
        }

        // GET: Canciones/Edit/5
        public ActionResult Edit(int? id)
        {
            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cancion cancion = db.Canciones
                .FirstOrDefault(c => c.CancionId == id.Value && c.UsuarioId == usuarioId.Value);

            if (cancion == null)
            {
                return HttpNotFound();
            }

            return View(cancion);
        }

        // POST: Canciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CancionId,Titulo,Artista,Genero,EstadoAnimo,ObjetivoEstudio,Duracion,Enlace")] Cancion cancion)
        {
            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            cancion.UsuarioId = usuarioId.Value;
            ModelState.Remove("UsuarioId");

            if (ModelState.IsValid)
            {
                var cancionOriginal = db.Canciones
                    .FirstOrDefault(c => c.CancionId == cancion.CancionId && c.UsuarioId == usuarioId.Value);

                if (cancionOriginal == null)
                {
                    return HttpNotFound();
                }

                cancionOriginal.Titulo = cancion.Titulo;
                cancionOriginal.Artista = cancion.Artista;
                cancionOriginal.Genero = cancion.Genero;
                cancionOriginal.EstadoAnimo = cancion.EstadoAnimo;
                cancionOriginal.ObjetivoEstudio = cancion.ObjetivoEstudio;
                cancionOriginal.Duracion = cancion.Duracion;
                cancionOriginal.Enlace = cancion.Enlace;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(cancion);
        }

        // GET: Canciones/Delete/5
        public ActionResult Delete(int? id)
        {
            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cancion cancion = db.Canciones
                .FirstOrDefault(c => c.CancionId == id.Value && c.UsuarioId == usuarioId.Value);

            if (cancion == null)
            {
                return HttpNotFound();
            }

            return View(cancion);
        }

        // POST: Canciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var usuarioId = ObtenerUsuarioId();

            if (usuarioId == null)
            {
                return RedirectToAction("Login", "Cuenta");
            }

            Cancion cancion = db.Canciones
                .FirstOrDefault(c => c.CancionId == id && c.UsuarioId == usuarioId.Value);

            if (cancion == null)
            {
                return HttpNotFound();
            }

            var relaciones = db.PlaylistCanciones
                .Where(pc => pc.CancionId == id && pc.Playlist.UsuarioId == usuarioId.Value)
                .ToList();

            db.PlaylistCanciones.RemoveRange(relaciones);

            db.Canciones.Remove(cancion);
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