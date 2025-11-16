using Kafen.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos
{
    public interface IRepositorioArticulo
    {
        IEnumerable<Articulo> ListarTodos();
        Articulo BuscarporId(Guid id);
        void Agregar(Articulo articulo);
        void Modificar(Articulo articulo);
        void Eliminar(Guid id);
    }
}
