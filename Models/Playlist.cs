using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListaReproduccionMVC.Models
{
    public class Playlist
    {
        [Key]
        public int PlaylistId { get; set; }
        [Required(ErrorMessage = "El nombre de la playlist es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<PlaylistCancion> PlaylistCanciones { get; set; }
    }
}