using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ListaReproduccionMVC.Models
{
    public class PlaylistCancion
    {
        [Key]
        public int PlaylistCancionId { get; set; }

        public int PlaylistId { get; set; }

        public int CancionId { get; set; }

        [ForeignKey("PlaylistId")]
        public virtual Playlist Playlist { get; set; }

        [ForeignKey("CancionId")]
        public virtual Cancion Cancion { get; set; }
    }
}