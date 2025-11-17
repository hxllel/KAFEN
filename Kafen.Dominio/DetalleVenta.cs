using Kafen.Dominio.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Dominio
{
    public class DetalleVenta : Entidad<Guid>
    {
        public DetalleVenta(Guid id) : base(id)
        {
            Id = id;
        }
        public Pedido IdPedido { get; set; }
        public Articulo IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        public void Actualizar(Pedido pedido, Articulo articulo, int cantidad, decimal precio)
        {
            IdPedido = pedido;
            IdProducto = articulo;
            Cantidad = cantidad;
            Precio = precio;

        }

        public static DetalleVenta Crear(Guid id, Pedido idpedido, Articulo idproducto, int cantidad, decimal precio)
        {
            DetalleVenta detalle = new DetalleVenta(id);
            detalle.Actualizar(idpedido, idproducto, cantidad, precio);

            return detalle;
        }


    }
}

