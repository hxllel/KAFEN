using Kafen.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public class RepositorioPedidos : IRepositorioPedidos
    {
        private readonly ContextoKafen _contexto;
        public RepositorioPedidos(ContextoKafen contexto)
        {
            _contexto = contexto;
        }
        public void Agregar(Pedido pedido)
        {
            _contexto.Add(pedido);
            _contexto.SaveChanges();
        }
        public Pedido BuscarporId(Guid id)
        {
            return _contexto.Pedidos
                .Include(a => a.Cliente)
                .Include(a => a.Estatus)
                .FirstOrDefault(ev => ev.Id == id);
        }

        public IEnumerable<Pedido> ListarTodos()
        {
            return _contexto.Pedidos
                .Include(a => a.Cliente)
                .Include(a => a.Estatus);


        }
        public void Modificar(Pedido pedido)
        {
            var pedidomodificar = _contexto.Pedidos
                .Find(pedido.Id);
            pedidomodificar.Actualizar(pedido.Cliente, pedido.Total, pedido.Estatus, pedido.Fecha);
            _contexto.SaveChanges();
        }
    }
}
