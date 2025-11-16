using Kafen.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Datos.Repositorios
{
    public interface IRepositorioCategoria
    {
        IEnumerable<Categoria> ListarTodas();
        Categoria BuscarPorId(int id);
        void Agregar(Categoria actividad);
        void Modificar(Categoria actividad);
        void Eliminar(int id);
    }
}
