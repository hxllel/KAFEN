using Kafen.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public interface IRepositorioPedidos
    {
        IEnumerable<Pedido> ListarTodos();
        Pedido BuscarporId(Guid id);
        void Agregar(Pedido pedido);
        void Modificar(Pedido pedido);

    }
}
