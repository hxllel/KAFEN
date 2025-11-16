using Kafen.Dominio;
using System;

namespace Kafen.Models
{
    public class PedidosViewModel
    {
        public Guid id { get; set; }
        public string ApPaterno { get; set; }
        public string ApMaterno { get; set; }
        public string Cliente { get; set; }
        public int Total { get; set; }
        public string Estatus { get; set; }

        public string Fecha { get; set; }

    }
}
