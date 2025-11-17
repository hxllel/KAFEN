using System;
using System.ComponentModel.DataAnnotations;

namespace Kafen.Models
{
    public class ArticuloEntrada
    {

        public Guid Id { get; set; }
        [Required(ErrorMessage = "Se necesita el Nombre del producto")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Se necesita la descripcion del producto")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Se necesita el precio del producto")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "Se necesita la categoria a la que pertenece el producto")]
        public int Categoria { get; set; }
    }
}
