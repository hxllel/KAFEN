using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Dominio
{
    public class Estatus
    {
        public Estatus(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public string descripcion { get; set; }


    }
}
