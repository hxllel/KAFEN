using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Transversal
{
    public interface IServicioEmail
    {
        void EnviarCorreo(string Correo_Electronico, string mensaje, string asunto);
    }
}
