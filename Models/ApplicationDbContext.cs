using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ListaReproduccionMVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cancion> Canciones { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistCancion> PlaylistCanciones { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Playlist>()
                .HasRequired(p => p.Usuario)
                .WithMany(u => u.Playlists)
                .HasForeignKey(p => p.UsuarioId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlaylistCancion>()
                .HasRequired(pc => pc.Playlist)
                .WithMany(p => p.PlaylistCanciones)
                .HasForeignKey(pc => pc.PlaylistId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlaylistCancion>()
                .HasRequired(pc => pc.Cancion)
                .WithMany()
                .HasForeignKey(pc => pc.CancionId)
                .WillCascadeOnDelete(false);
        }
    }
}