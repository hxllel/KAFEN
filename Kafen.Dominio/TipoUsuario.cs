using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Dominio
{
    public class TipoUsuario
    {
        public TipoUsuario(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public string Descripcion { get; set; }


    }
}
