using Kafen.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public class RepositorioDetalleVenta : IRepositorioDetalleVenta
    {
        private readonly ContextoKafen _contexto;
        public RepositorioDetalleVenta(ContextoKafen contexto)
        {
            _contexto = contexto;
        }
        public void Agregar(DetalleVenta detalleVenta)
        {
            _contexto.Add(detalleVenta);
            _contexto.SaveChanges();
        }
        public void Modificar(DetalleVenta detalleVenta)
        {
            var detallemodificar = _contexto.Detalleventa
                .Find(detalleVenta.Id);
            detallemodificar.Actualizar(detalleVenta.IdPedido, detalleVenta.IdProducto, detalleVenta.Cantidad, detalleVenta.Precio);
            _contexto.SaveChanges();
        }

        public IEnumerable<DetalleVenta> ListarTodos()
        {
            return _contexto.Detalleventa
                .Include(a => a.IdPedido)
                .Include(a => a.IdProducto);


        }

    }
}
