using Kafen.Datos.Repositorios;
using System.Linq;
using Kafen.Models;
using System.Security.Cryptography;
using System.Text;

namespace Kafen.Data
{
    public class DA_Logica
    {
        private readonly IRepositorioUsuario _repositorioUsuario;
        public DA_Logica(IRepositorioUsuario repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }
        public Usuario ValidarUsuario(int _Id, string contraseña)
        {


            return _repositorioUsuario.ListarTodos().Where(a => a.Id == _Id && a.Contraseña == contraseña).Select(a => new Usuario { }).FirstOrDefault();
            ;

        }
   
    }
}
