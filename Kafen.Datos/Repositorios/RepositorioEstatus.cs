using Kafen.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public class RepositorioEstatus : IRepositorioEstatus
    {
        private readonly ContextoKafen _contexto;
        public RepositorioEstatus(ContextoKafen contexto)
        {
            _contexto = contexto;
        }

        public Estatus BuscarporId(int id)
        {
            return _contexto.Estatus.Find(id);
        }
    }
}
