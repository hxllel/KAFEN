using Kafen.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public interface IRepositorioDetalleVenta
    {

        void Agregar(DetalleVenta detalleVenta);
        void Modificar(DetalleVenta detalleVenta);
        IEnumerable<DetalleVenta> ListarTodos();
    }
}
