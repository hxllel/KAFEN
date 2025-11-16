using System;

namespace Kafen.Models
{
    public class DetalleVentaViewModel
    {

        public Guid IdPedido { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public int Precio { get; set; }
    }
}
