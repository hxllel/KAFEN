using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Transversal
{
    public class ServicioEmail : IServicioEmail
    {
        public void EnviarCorreo(string Correo_Electronico, string mensaje, string asunto)
        {

            MailMessage correo = new MailMessage();
            correo.From = new MailAddress("kanfe.asistencia@gmail.com");
            correo.To.Add(Correo_Electronico);
            correo.Subject = asunto;
            correo.Body = mensaje;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            string ScuentaCorreo = "kanfe.asistencia@gmail.com";
            string SpasswordCorreo = "Kanfe1234";
            smtp.Credentials = new System.Net.NetworkCredential(ScuentaCorreo, SpasswordCorreo);
            smtp.Send(correo);


        }
    }
}
