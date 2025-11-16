using Kafen.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public class RepositorioArticulo : IRepositorioArticulo
    {
        private readonly ContextoKafen _contexto;
        public RepositorioArticulo(ContextoKafen contexto)
        {
            _contexto = contexto;
        }
        public void Agregar(Articulo articulo)
        {
            _contexto.Add(articulo);
            _contexto.SaveChanges();
        }
        public Articulo BuscarporId(Guid id)
        {
            return _contexto.Articulos
                .Include(a => a.Categoria)
                .FirstOrDefault(ev => ev.Id == id);
        }
        public void Eliminar(Guid id)
        {
            var articuloeliminar = _contexto.Articulos.Find(id);
            _contexto.Articulos.Remove(articuloeliminar);
            _contexto.SaveChanges();

        }
        public IEnumerable<Articulo> ListarTodos()
        {
            return _contexto.Articulos
                .Include(a => a.Categoria);

        }
        public void Modificar(Articulo articulo)
        {
            var articulomodificar = _contexto.Articulos
                .Find(articulo.Id);
            articulomodificar.Actualizar(articulo.Nombre, articulo.Descripcion, articulo.Precio, articulo.Categoria);
            _contexto.SaveChanges();
        }
    }
}
