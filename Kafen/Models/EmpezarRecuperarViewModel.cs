using System.ComponentModel.DataAnnotations;

namespace Kafen.Models
{
    public class EmpezarRecuperarViewModel
    {
        [EmailAddress(ErrorMessage = "El formato es incorrecto")]
        [Required(ErrorMessage = "El campo es obligatorio")]
        public string Correo_Electronico { get; set; }
    }
}
