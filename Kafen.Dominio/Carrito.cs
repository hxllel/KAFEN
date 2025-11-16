using Kafen.Dominio.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Dominio
{
    public class Carrito : Entidad<Guid>
    {
        public Carrito(Guid id) : base(id)
        {
            Id = id;
        }
        public Articulo IdArticulo { get; set; }
        public Usuario IdUsuario { get; set; }

        public int Cantidad { get; set; }

        public void Actualizar(Articulo articulo, Usuario usuario, int cantidad)
        {

            IdArticulo = articulo;
            IdUsuario = usuario;
            Cantidad = cantidad;
        }
        public static Carrito Crear(Guid id, Articulo idArticulo, Usuario idUsuario, int cantidad)
        {
            Carrito carrito = new Carrito(id);
            carrito.Actualizar(idArticulo, idUsuario, cantidad);
            return carrito;
        }

    }
}
