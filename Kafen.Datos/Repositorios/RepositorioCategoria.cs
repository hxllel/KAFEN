using Kafen.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public class RepositorioCategoria : IRepositorioCategoria
    {
        private readonly ContextoKafen _contexto;
        public RepositorioCategoria(ContextoKafen contexto)
        {
            _contexto = contexto;
        }

        public void Agregar(Categoria actividad)
        {
            _contexto.Add(actividad);
            _contexto.SaveChanges();
        }

        public Categoria BuscarPorId(int id)
        {
            return _contexto.Categoria.Find(id);
        }

        public void Eliminar(int id)
        {
            var actividadEliminar = _contexto.Categoria.Find(id);
            _contexto.Categoria.Remove(actividadEliminar);
            _contexto.SaveChanges();
        }

        public IEnumerable<Categoria> ListarTodas()
        {
            return _contexto.Categoria.OrderBy(a => a.Nombre);
        }

        public void Modificar(Categoria categoria)
        {
            var categoriaModificar = _contexto.Categoria
                .Find(categoria.Id);
            categoriaModificar.Actualizar(
                categoria.Nombre);
            _contexto.SaveChanges();
        }
    }
}
