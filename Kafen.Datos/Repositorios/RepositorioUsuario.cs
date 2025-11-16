using Kafen.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private readonly ContextoKafen _contexto;
        public RepositorioUsuario(ContextoKafen contexto)
        {
            _contexto = contexto;
        }
        public void Agregar(Usuario usuario)
        {
            _contexto.Add(usuario);
            _contexto.SaveChanges();
        }
        public Usuario BuscarporId(int id)
        {
            return _contexto.Usuarios.Find(id);

        }
        public IEnumerable<Usuario> ListarTodos()
        {
            return _contexto.Usuarios.OrderBy(a => a.Nombre);
        }
        /*public void Modificar(Usuario usuario)
         {
             var usuariomodificar = _contexto.Usuarios
                 .Find(usuario.Id);
             usuariomodificar.Actualizar(usuario.Nombre, usuario.Apellido_Paterno, articulo.Precio, articulo.Categoria);
             _contexto.SaveChanges();
         }*/
    }
}
