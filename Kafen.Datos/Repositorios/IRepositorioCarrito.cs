using Kafen.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public interface IRepositorioCarrito
    {
        IEnumerable<Carrito> ListarTodos();
        void Agregar(Carrito carrito);
        void Eliminar(Guid id);

    }
}
