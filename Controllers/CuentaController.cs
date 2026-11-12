using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ListaReproduccionMVC.Models;
using ListaReproduccionMVC.Models.ViewModels;

namespace ListaReproduccionMVC.Controllers
{
    public class CuentaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // Aqui se va a registrar las cuentas del usuario

        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool correoExiste = db.Usuarios.Any(u => u.Correo == model.Correo);


                if (correoExiste)
                {
                    ModelState.AddModelError("Correo", "Este correo ya está registrado.");
                    return View(model);
                }

                Usuario usuario = new Usuario
                {
                    Nombre = model.Nombre,
                    Correo = model.Correo.Trim(),
                    Password = model.Password.Trim(),
                    GeneroFavorito = model.GeneroFavorito,
                    ObjetivoEstudio = model.ObjetivoEstudio
                };

                db.Usuarios.Add(usuario);
                db.SaveChanges();

                TempData["Mensaje"] = "Usuario Registrado exitosamente! Ahora puede iniciar sesión.";
                return RedirectToAction("Login");
            }
                return View(model);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login (LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = db.Usuarios.FirstOrDefault(u => 
                u.Correo == model.Correo && 
                u.Password == model.Password);

                if (usuario != null)
                {
                    Session["UsuarioId"] = usuario.UsuarioId;
                    Session["UsuarioNombre"] = usuario.Nombre;

                    return RedirectToAction("Index", "Canciones");
                }

                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
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