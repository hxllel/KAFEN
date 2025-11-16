using System.ComponentModel.DataAnnotations;
namespace Kafen.Models
{
    public class RecuperarViewModel
    {
        [Required(ErrorMessage = "Se necesita la contraseña")]

        public string Contraseña { get; set; }

        [Required(ErrorMessage = "Se necesita que repita la contraseña")]
        [Compare("Contraseña", ErrorMessage = "No coinciden las contraseñas")]
        public string ConfirmarContraseña { get; set; }
    }
}
