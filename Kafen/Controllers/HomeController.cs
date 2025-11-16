using Kafen.Models;
using Kafen.Transversal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Kafen.Data;
using Kafen.Datos.Repositorios;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Kafen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositorioUsuario _repositorioUsuario;
        public HomeController(IRepositorioUsuario repositorioUsuario, ILogger<HomeController> logger)
        {
            _repositorioUsuario = repositorioUsuario;
            _logger = logger;

        }
        public static bool Menor = false;
        public string token3;
        public static int ID;
        public static string linkreset;
        public string urldomain = "http://localhost:24497/";
        static string token2;
        static public string correo;
        static bool Duplicado = false;
        static bool Registrado = false;
        static bool CorreoDuplicado = false;
        static bool NoEncontrado = false;
        static bool Enviado = false;
        public static bool CambioContra = false;
        static bool tokenulo = false;

        static string cadena = "workstation id=KafenData.mssql.somee.com;packet size=4096;user id=Kafen_SQLLogin_1;pwd=u1nxeltc55;data source=KafenData.mssql.somee.com;persist security info=False;initial catalog=KafenData";

        public IActionResult Index(bool registrado = false)
        {
         
            if (Menor)
            {
                ViewData["Menor"] = "No puedes registrarte siendo menor de 12 años";
            }
            if (tokenulo)
            {
                ViewData["Tokenulo"] = "El token es nulo o ha expirado";
            }
            if (registrado == true)
            {
                Registrado = true;
            }
            if (Enviado)
            {
                ViewData["Enviado"] = "Se ha enviado un correo con instrucciones";
            }

            if (CambioContra)
            {
                ViewData["CambioContra"] = "Se ha cambiado la contraseña correctamente";
            }
            if (Duplicado)
            {
                ViewData["Duplicado"] = "ESTE ID YA EXISTE!";
            }
            if (Registrado)
            {
                ViewData["Registrado"] = "USUARIO REGISTRADO CON EXITO";
            }
            if (NoEncontrado)
            {
                ViewData["NoEncontrado"] = "Usuario o contraseña no encontrada";
            }
            if (CorreoDuplicado)
            {
                ViewData["CorreoDuplicado"] = "El correo ya esta en uso!";
            }
            Menor = false;
            Enviado = false;
            Duplicado = false;
            Registrado = false;
            NoEncontrado = false;
            CorreoDuplicado = false;
            CambioContra = false;
            tokenulo = false;
            return View();

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario oUsuario)
        {
            DA_Logica _dausuario = new DA_Logica(_repositorioUsuario);
            var usuario = _dausuario.ValidarUsuario(oUsuario.Id, oUsuario.Contraseña);





            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("Id", oUsuario.Id);

                cmd.Parameters.AddWithValue("Contraseña", oUsuario.Contraseña);

                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();

                SqlDataReader Rleido;
                Rleido = cmd.ExecuteReader();
                int filas = Convert.ToInt32(Rleido.HasRows);
                try
                {
                    if (Rleido.Read())
                    {
                        if (Convert.ToInt32(Rleido[1]) == 1)
                        {
                             var claims = new List<Claim> {
                                  new Claim("Id", usuario.Id.ToString()),

                            };

                            claims.Add(new Claim(ClaimTypes.Role, Rleido[1].ToString()));

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                            return RedirectToAction("Tienda", "Cliente", new RouteValueDictionary(new { usuario = oUsuario.Id }));
                        }

                        if (Convert.ToInt32(Rleido[1]) == 2)
                        {
                            var claims = new List<Claim> {
                                  new Claim("Id", usuario.Id.ToString()),

                            };

                            claims.Add(new Claim(ClaimTypes.Role, Rleido[1].ToString()));

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                            return RedirectToAction("Pedidos", "Vendedor");


                        }
                    }
                    
                }
                catch (IndexOutOfRangeException)
                {
                    NoEncontrado = true;

                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "Home");
            }


        }

        public IActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registro(Usuario oUsuario)
        {
            if (ModelState.IsValid)
            {
                string diahoy = DateTime.Today.ToString("yyyy");
                int DIAHOY = Convert.ToInt32(diahoy) - 12;
                string diaNacimiento = oUsuario.Fecha_Nacimiento.ToString("yyyy");
                int DIANACIMIENTO = Convert.ToInt32(diaNacimiento);

                if (DIANACIMIENTO >= DIAHOY)
                {

                    Menor = true;
                    return RedirectToAction("Index", "Home");


                }



                else
                {
                    using (SqlConnection cm = new SqlConnection(cadena))
                    {
                        SqlCommand cmd = new SqlCommand("ValidarCorreo", cm);
                        cmd.Parameters.AddWithValue("Correo", oUsuario.Correo_Electronico);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cm.Open();

                        SqlDataReader Rleido;
                        Rleido = cmd.ExecuteReader();
                        int filas = Convert.ToInt32(Rleido.HasRows);

                        if (Rleido.Read())
                        {
                            CorreoDuplicado = true;
                            return RedirectToAction("Index", "Home");
                        }

                        else
                        {

                            try
                            {
                                string Nac = oUsuario.Fecha_Nacimiento.ToString("d");
                                string Reg = DateTime.Today.ToString("d");

                                using (SqlConnection cn = new SqlConnection(cadena))
                                {


                                    SqlCommand cnd = new SqlCommand("RegistroUsuario", cn);
                                    cnd.Parameters.AddWithValue("Id", Convert.ToInt32(oUsuario.Id));
                                    cnd.Parameters.AddWithValue("Apellido_Paterno", Convert.ToString(oUsuario.Apellido_Paterno));
                                    cnd.Parameters.AddWithValue("Apellido_Materno", oUsuario.Apellido_Materno.ToString());
                                    cnd.Parameters.AddWithValue("Nombre", oUsuario.Nombre.ToString());
                                    cnd.Parameters.AddWithValue("Contraseña", oUsuario.Contraseña.ToString());
                                    cnd.Parameters.Add("@TipoUsuario", SqlDbType.Int).Value = 1;
                                    cnd.Parameters.AddWithValue("FechaRegistro", Convert.ToDateTime(Reg));
                                    cnd.Parameters.AddWithValue("FechaNacimiento", Convert.ToDateTime(Nac));
                                    cnd.Parameters.AddWithValue("Correo", oUsuario.Correo_Electronico.ToString());

                                    cnd.CommandType = CommandType.StoredProcedure;
                                    cn.Open();
                                    cnd.ExecuteNonQuery();

                                    cn.Close();

                                }
                              string mensaje = "Se ha creado exitosamente su cuenta de Kanfe, ya puede empezar a comprar";
                                string asunto = "Se creo su cuenta de Kafen";
                                ServicioEmail EnvioCorreo = new ServicioEmail();
                                EnvioCorreo.EnviarCorreo(oUsuario.Correo_Electronico.ToString(), mensaje, asunto);
                            }
                            catch (Microsoft.Data.SqlClient.SqlException)
                            {
                                Duplicado = true;

                                return RedirectToAction("Index", "Home");
                            }

                        }
                        Registrado = true;

                        return RedirectToAction("Index", "Home");

                    }
                }
            }
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public IActionResult EmpezarRecuperar(string token)
        {
            return View();
        }
        [HttpPost]

        public IActionResult EmpezarRecuperar(EmpezarRecuperarViewModel model)
        {
            string host = Request.Host.ToString();
            if (ModelState.IsValid)
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("ValidarCorreo", cn);
                    cmd.Parameters.AddWithValue("Correo", model.Correo_Electronico);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();

                    SqlDataReader Rleido;
                    Rleido = cmd.ExecuteReader();
                    int filas = Convert.ToInt32(Rleido.HasRows);

                    if (Rleido.Read())
                    {
                        correo = Convert.ToString(Rleido[0]);
                        token2 = Guid.NewGuid().ToString();
                        linkreset = "http://"+ host+"/" + "Home/Recuperar/" + token2;

                        string mensaje = "Para reestablecer su contraseña favor de ingresar a este link <a href = '" + linkreset + "'>Click para recuperar</a>";
                        string asunto = "Cambio de contraseña";
                        ServicioEmail EnvioCorreo = new ServicioEmail();
                        EnvioCorreo.EnviarCorreo(model.Correo_Electronico.ToString(), mensaje, asunto);
                        Enviado = true;
                        cn.Close();
                        Rleido.Close();

                        return RedirectToAction("Index", "Home");

                    }

                    else
                    {
                        NoEncontrado = true;

                        return RedirectToAction("Index", "Home");
                    }





                }
            }



            return View();
        }

        [HttpGet]
        public IActionResult Recuperar(string token)
        {
            if (token2 == null)
            {

                tokenulo = true;
                return RedirectToAction("Index", "Home");
            }
            token3 = token2;
            return View();
        }

        [HttpPost]
        public IActionResult Recuperar(RecuperarViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("ValidarToken", cn);
                    cmd.Parameters.AddWithValue("Correo", correo);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();

                    SqlDataReader Rleido;
                    Rleido = cmd.ExecuteReader();
                    int filas = Convert.ToInt32(Rleido.HasRows);

                    if (Rleido.Read())
                    {

                        ID = Convert.ToInt32(Rleido[1]);
                        cn.Close();
                        Rleido.Close();
                        using (SqlConnection cm = new SqlConnection(cadena))
                        {
                            SqlCommand cnd = new SqlCommand("CambiarContra", cm);
                            cnd.Parameters.AddWithValue("Id", Convert.ToInt32(ID));
                            cnd.Parameters.AddWithValue("Contraseña", Convert.ToString(model.Contraseña));


                            cnd.CommandType = CommandType.StoredProcedure;
                            cm.Open();
                            cnd.ExecuteNonQuery();

                            cm.Close();
                            string mensaje = "Se reestablecio su contraseña exitosamente";
                            string asunto = "Cambio de contraseña";
                            ServicioEmail EnvioCorreo = new ServicioEmail();
                            EnvioCorreo.EnviarCorreo(correo, mensaje, asunto);
                            CambioContra = true;
                            token2 = null;
                            return RedirectToAction("Index", "Home");


                        }

                    }
                }
            }
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

     


    }
}
