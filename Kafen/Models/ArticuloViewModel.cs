using System;

namespace Kafen.Models
{
    public class ArticuloViewModel
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
        public string Categoria { get; set; }
    }
}
