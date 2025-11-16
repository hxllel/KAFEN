using System;
using System.ComponentModel.DataAnnotations;

namespace Kafen.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "Se necesita el id")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Se necesita el apellido paterno")]
        public string Apellido_Paterno { get; set; }
        [Required(ErrorMessage = "Se necesita el apellido materno")]
        public string Apellido_Materno { get; set; }
        [Required(ErrorMessage = "Se necesita el nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Se necesita la contraseña")]
        public string Contraseña { get; set; }
        public int tipo_UsuarioId { get; set; }
        public DateTime Fecha_Registro { get; set; }
        [Required(ErrorMessage = "Se necesita la fecha de nacimiento")]
        public DateTime Fecha_Nacimiento { get; set; }
        [Required(ErrorMessage = "Se necesita el correo electronico")]
        public string Correo_Electronico { get; set; }


    }
}
