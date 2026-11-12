using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListaReproduccionMVC.Models
{
    public class Cancion
    {
        [Key]
        public int CancionId { get; set; }

        [Required(ErrorMessage = "El titulo es obligatorio.")]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El artista es obligatorio.")]
        [StringLength(100)]
        public string Artista { get; set; }

        [Required(ErrorMessage = "El género es obligatorio")]
        [StringLength(50)]
        public string Genero { get; set; }

        [Required(ErrorMessage = "El estado de ánimo es necesario.")]
        [Display(Name = "Estado de ánimo")]
        public string EstadoAnimo { get; set; }

        [Required(ErrorMessage = "El Objetivo de estudio es obligatorio.")]
        [Display(Name = "Objetivo de estudio")]
        public string ObjetivoEstudio { get; set; }

        [Display(Name = "Duración")]
        [StringLength(10)]
        public string Duracion { get; set; }

        [Display(Name = "Ruta de cancion")]
        [StringLength(250)]
        public string Enlace {  get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
}