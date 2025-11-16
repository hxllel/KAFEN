using Kafen.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public class RepositorioCarrito : IRepositorioCarrito
    {

        private readonly ContextoKafen _contexto;
        public RepositorioCarrito(ContextoKafen contexto)
        {
            _contexto = contexto;
        }
        public IEnumerable<Carrito> ListarTodos()
        {
            return _contexto.Carrito
                .Include(a => a.IdArticulo)
                .Include(a => a.IdUsuario);

        }
        public void Agregar(Carrito articulo)
        {
            _contexto.Add(articulo);
            _contexto.SaveChanges();
        }
        public void Eliminar(Guid id)
        {
            var carritoeliminar = _contexto.Carrito.Find(id);
            _contexto.Carrito.Remove(carritoeliminar);
            _contexto.SaveChanges();

        }
    }
}
