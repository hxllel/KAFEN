using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Dominio
{
    public class Usuario
    {
        public Usuario(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public string Apellido_Paterno { get; protected set; }
        public string Apellido_Materno { get; protected set; }
        public string Nombre { get; protected set; }
        public string Contraseña { get; protected set; }
        public TipoUsuario tipo_Usuario { get; protected set; }
        public DateTime FechaRegistro { get; protected set; }
        public DateTime FechaNacimiento { get; protected set; }
        public string Correo_Electronico { get; protected set; }


        public void Actualizar(string contraseña, string correo_electronico)
        {
            if (string.IsNullOrEmpty(correo_electronico))
                throw new ArgumentException("El correo es necesario");
            Correo_Electronico = correo_electronico;
        }
        public static Usuario Crear(int id, string apellido_paterno,
            string apellido_materno, string nombre, int indentificador, string contraseña,
            TipoUsuario tipo_usuario, DateTime fecharegistro, DateTime fechanacimiento,
            string correo_electronico)
        {
            Usuario usuario = new Usuario(id);
            usuario.Actualizar(contraseña, correo_electronico);
            return usuario;
        }
    }
}
