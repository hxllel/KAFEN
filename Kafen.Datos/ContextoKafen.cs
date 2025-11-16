using Kafen.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos
{
    public class ContextoKafen : DbContext
    {
        public ContextoKafen(DbContextOptions<ContextoKafen>
            options) : base(options)
        {

        }

        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<TipoUsuario> Tipousuario { get; set; }
        public DbSet<DetalleVenta> Detalleventa { get; set; }
        public DbSet<Estatus> Estatus { get; set; }
        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //-------------------------TIPO USUARIP--------------------------------------------------------

            modelBuilder.Entity<TipoUsuario>().Property(a => a.Descripcion).IsRequired();
            modelBuilder.Entity<TipoUsuario>().Property(a => a.Descripcion).HasMaxLength(20);

            //-------------------------PRODUCTOS------------------------------------------------------------

            modelBuilder.Entity<Articulo>().Property(a => a.Nombre).IsRequired();
            modelBuilder.Entity<Articulo>().Property(a => a.Nombre).HasMaxLength(20);
            modelBuilder.Entity<Articulo>().Property(a => a.Descripcion).IsRequired();
            modelBuilder.Entity<Articulo>().Property(a => a.Descripcion).HasMaxLength(100);
            modelBuilder.Entity<Articulo>().Property(a => a.Precio).IsRequired();
            modelBuilder.Entity<Articulo>().Property(a => a.Precio).HasMaxLength(15);

            //-------------------------CLIENTE--------------------------------------------------------------
            modelBuilder.Entity<Usuario>().Property(a => a.Contraseña).IsRequired();
            modelBuilder.Entity<Usuario>().Property(a => a.Contraseña).HasMaxLength(12);
            modelBuilder.Entity<Usuario>().Property(a => a.Nombre).IsRequired();
            modelBuilder.Entity<Usuario>().Property(a => a.Apellido_Paterno).IsRequired();
            modelBuilder.Entity<Usuario>().Property(a => a.Apellido_Materno).IsRequired();
            modelBuilder.Entity<Usuario>().Property(a => a.FechaRegistro).IsRequired();
            modelBuilder.Entity<Usuario>().Property(a => a.FechaNacimiento).IsRequired();
            modelBuilder.Entity<Usuario>().Property(a => a.Correo_Electronico).IsRequired();
            modelBuilder.Entity<Usuario>().Property(a => a.Correo_Electronico).HasMaxLength(50);


            //-------------------------ESTATUS--------------------------------------------------------------
            modelBuilder.Entity<Estatus>().Property(a => a.descripcion).IsRequired();
            modelBuilder.Entity<Estatus>().Property(a => a.descripcion).HasMaxLength(20);
            //-------------------------PEDIDOS------------------------------------------------------------


            modelBuilder.Entity<Pedido>().Property(a => a.Fecha).IsRequired();
            //-------------------------DETALLE PEDIDO--------------------------------------------------------

            modelBuilder.Entity<DetalleVenta>().Property(a => a.Cantidad).IsRequired();
            modelBuilder.Entity<DetalleVenta>().Property(a => a.Precio).IsRequired();

        }

    }
}
