using Kafen.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public interface IRepositorioUsuario
    {
        IEnumerable<Usuario> ListarTodos();
        Usuario BuscarporId(int id);
        void Agregar(Usuario usuario);
        //void Modificar(Usuario usuario);
    }
}
