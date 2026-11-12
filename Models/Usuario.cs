using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ListaReproduccionMVC.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [StringLength(100)]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(50)]
        public string GeneroFavorito { get; set; }

        [StringLength (50)]
        public string ObjetivoEstudio { get; set; }

        public virtual ICollection<Cancion> Canciones { get; set; }

        public virtual ICollection<Playlist> Playlists { get; set; }
    }
}