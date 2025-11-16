using System;
using System.ComponentModel.DataAnnotations;

namespace Kafen.Models
{
    public class CarritoVista
    {
        
        public Guid Id { get; set; }
        [Required(ErrorMessage = "No puede hacer pedidos sin nada en el carrito")]

        public string Producto { get; set; }
        [Required(ErrorMessage = "No puede hacer pedidos sin nada en el carrito")]

        public int Precio { get; set; }
        [Required(ErrorMessage = "No puede hacer pedidos sin nada en el carrito")]

        public string Descripcion { get; set; }
        [Required(ErrorMessage = "No puede hacer pedidos sin nada en el carrito")]
        public int Cantidad { get; set; }

    }
}
