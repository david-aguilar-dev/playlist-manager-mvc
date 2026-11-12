using ListaReproduccionMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace ListaReproduccionMVC.Controllers
{
    public class PlaylistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private bool UsuarioNoLogueado()
        {
            return Session["UsuarioId"] == null;
        }

        private int ObtenerUsuarioId()
        {
            return Convert.ToInt32(Session["UsuarioId"]);
        }

        // GET: Playlists
        public ActionResult Index()
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            int usuarioId = ObtenerUsuarioId();

            var playlists = db.Playlists
                .Where(p => p.UsuarioId == usuarioId)
                .ToList();

            return View(playlists);
        }

        // GET: Playlists/Details/5
        public ActionResult Details(int? id)
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int usuarioId = ObtenerUsuarioId();

            var playlist = db.Playlists
                .Include(p => p.PlaylistCanciones.Select(pc => pc.Cancion))
                .FirstOrDefault(p => p.PlaylistId == id && p.UsuarioId == usuarioId);

            if (playlist == null)
            {
                return HttpNotFound();
            }

            return View(playlist);
        }

        // GET: Playlists/Create
        public ActionResult Create()
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            return View();
        }

        // POST: Playlists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Descripcion")] Playlist playlist)
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (ModelState.IsValid)
            {
                playlist.UsuarioId = ObtenerUsuarioId();

                db.Playlists.Add(playlist);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(playlist);
        }

        // GET: Playlists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int usuarioId = ObtenerUsuarioId();

            var playlist = db.Playlists
                .FirstOrDefault(p => p.PlaylistId == id && p.UsuarioId == usuarioId);

            if (playlist == null)
            {
                return HttpNotFound();
            }

            return View(playlist);
        }

        // POST: Playlists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Playlist playlist)
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (ModelState.IsValid)
            {
                int usuarioId = ObtenerUsuarioId();

                var playlistExistente = db.Playlists
                    .FirstOrDefault(p => p.PlaylistId == playlist.PlaylistId && p.UsuarioId == usuarioId);

                if (playlistExistente == null)
                {
                    return HttpNotFound();
                }

                playlistExistente.Nombre = playlist.Nombre;
                playlistExistente.Descripcion = playlist.Descripcion;

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(playlist);
        }

        // GET: Playlists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int usuarioId = ObtenerUsuarioId();

            var playlist = db.Playlists
                .FirstOrDefault(p => p.PlaylistId == id && p.UsuarioId == usuarioId);

            if (playlist == null)
            {
                return HttpNotFound();
            }

            return View(playlist);
        }

        // POST: Playlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            int usuarioId = ObtenerUsuarioId();

            var playlist = db.Playlists
                .FirstOrDefault(p => p.PlaylistId == id && p.UsuarioId == usuarioId);

            if (playlist == null)
            {
                return HttpNotFound();
            }

            var relaciones = db.PlaylistCanciones
                .Where(pc => pc.PlaylistId == id)
                .ToList();

            db.PlaylistCanciones.RemoveRange(relaciones);
            db.Playlists.Remove(playlist);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Playlists/AgregarCancion/5
        public ActionResult AgregarCancion(int? id)
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int usuarioId = ObtenerUsuarioId();

            var playlist = db.Playlists
                .FirstOrDefault(p => p.PlaylistId == id && p.UsuarioId == usuarioId);

            if (playlist == null)
            {
                return HttpNotFound();
            }

            var cancionesYaAgregadas = db.PlaylistCanciones
                .Where(pc => pc.PlaylistId == id)
                .Select(pc => pc.CancionId)
                .ToList();

            var cancionesDisponibles = db.Canciones
                .Where(c => c.UsuarioId == usuarioId && !cancionesYaAgregadas.Contains(c.CancionId))
                .ToList();

            ViewBag.PlaylistId = playlist.PlaylistId;
            ViewBag.PlaylistNombre = playlist.Nombre;

            return View(cancionesDisponibles);
        }

        // POST: Playlists/AgregarCancion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarCancion(int playlistId, int cancionId)
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            int usuarioId = ObtenerUsuarioId();

            var playlist = db.Playlists
                .FirstOrDefault(p => p.PlaylistId == playlistId && p.UsuarioId == usuarioId);

            if (playlist == null)
            {
                return HttpNotFound();
            }

            var cancion = db.Canciones
                .FirstOrDefault(c => c.CancionId == cancionId && c.UsuarioId == usuarioId);

            if (cancion == null)
            {
                return HttpNotFound();
            }

            bool yaExiste = db.PlaylistCanciones
                .Any(pc => pc.PlaylistId == playlistId && pc.CancionId == cancionId);

            if (!yaExiste)
            {
                var playlistCancion = new PlaylistCancion
                {
                    PlaylistId = playlistId,
                    CancionId = cancionId
                };

                db.PlaylistCanciones.Add(playlistCancion);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = playlistId });
        }

        // POST: Playlists/QuitarCancion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuitarCancion(int playlistCancionId)
        {
            if (UsuarioNoLogueado())
            {
                return RedirectToAction("Login", "Cuenta");
            }

            int usuarioId = ObtenerUsuarioId();

            var relacion = db.PlaylistCanciones
                .Include(pc => pc.Playlist)
                .FirstOrDefault(pc => pc.PlaylistCancionId == playlistCancionId &&
                                      pc.Playlist.UsuarioId == usuarioId);

            if (relacion == null)
            {
                return HttpNotFound();
            }

            int playlistId = relacion.PlaylistId;

            db.PlaylistCanciones.Remove(relacion);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = playlistId });
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