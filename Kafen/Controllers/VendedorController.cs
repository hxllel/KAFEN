using Kafen.Datos;
using Kafen.Datos.Repositorios;
using Kafen.Models;
using Kafen.Transversal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kafen.Controllers
{
    public class VendedorController : Controller
    {

        private readonly IRepositorioArticulo _repositorioArticulo;
        private readonly IRepositorioCategoria _repositorioCategoria;
        private readonly IRepositorioCarrito _repositorioCarrito;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IRepositorioPedidos _repositorioPedidos;
        private readonly IRepositorioEstatus _repositorioEstatus;
        private readonly IRepositorioDetalleVenta _repositorioDetalleVenta;
        public VendedorController(IRepositorioArticulo repositorioArticulo,
            IRepositorioCategoria repositorioCategoria, IRepositorioCarrito repositorioCarrito
            , IRepositorioUsuario repositorioUsuario, IRepositorioPedidos repositorioPedidos, IRepositorioEstatus repositorioEstatus
            , IRepositorioDetalleVenta repositorioDetalleVenta)
        {
            _repositorioArticulo = repositorioArticulo;
            _repositorioCategoria = repositorioCategoria;
            _repositorioCarrito = repositorioCarrito;
            _repositorioUsuario = repositorioUsuario;
            _repositorioPedidos = repositorioPedidos;
            _repositorioEstatus = repositorioEstatus;
            _repositorioDetalleVenta = repositorioDetalleVenta;
        }

        static bool Menor = false;
        static bool tope = false;
        static bool succesfull = false;
        static bool CorreoDuplicado = false;
        static bool Duplicado = false;
        static string cadena = "workstation id=KafenData.mssql.somee.com;packet size=4096;user id=Kafen_SQLLogin_1;pwd=u1nxeltc55;data source=KafenData.mssql.somee.com;persist security info=False;initial catalog=KafenData";
        public IActionResult Index()
        {
            if (Menor)
            {
                ViewData["Menor"] = "No puedes registrate como vendedor siendo menor de edad";
            }
            if (CorreoDuplicado)
            {
                ViewData["Correo"] = "El correo ya esta en uso";
            }
            if (Duplicado)
            {
                ViewData["Id"] = "El id ya esta registrado!";
            }
            Menor = false;
            CorreoDuplicado = false;
            Duplicado = false;
            return View();
        }
        [Authorize(Roles = "2")]
        public IActionResult VendedorInicio()
        {
            return View();
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
                int DIAHOY = Convert.ToInt32(diahoy) - 18;
                string diaNacimiento = oUsuario.Fecha_Nacimiento.ToString("yyyy");
                int DIANACIMIENTO = Convert.ToInt32(diaNacimiento);

                if (DIANACIMIENTO >= DIAHOY)
                {

                    Menor = true;
                    return RedirectToAction("Index", "Vendedor");


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
                            return RedirectToAction("Index", "Vendedor");
                        }

                        else
                        {

                            try
                            {

                                using (SqlConnection cn = new SqlConnection(cadena))
                                {
                                    SqlCommand cnd = new SqlCommand("RegistroUsuario", cn);
                                    cnd.Parameters.AddWithValue("Id", Convert.ToInt32(oUsuario.Id));
                                    cnd.Parameters.AddWithValue("Apellido_Paterno", Convert.ToString(oUsuario.Apellido_Paterno));
                                    cnd.Parameters.AddWithValue("Apellido_Materno", oUsuario.Apellido_Materno.ToString());
                                    cnd.Parameters.AddWithValue("Nombre", oUsuario.Nombre.ToString());
                                    cnd.Parameters.AddWithValue("Contraseña", oUsuario.Contraseña.ToString());
                                    cnd.Parameters.Add("@TipoUsuario", SqlDbType.Int).Value = 2;
                                    cnd.Parameters.AddWithValue("FechaRegistro", DateTime.Today);
                                    cnd.Parameters.AddWithValue("FechaNacimiento", Convert.ToDateTime(oUsuario.Fecha_Nacimiento));
                                    cnd.Parameters.AddWithValue("Correo", oUsuario.Correo_Electronico.ToString());

                                    cnd.CommandType = CommandType.StoredProcedure;
                                    cn.Open();
                                    cnd.ExecuteNonQuery();

                                    cn.Close();
                                }
                                string mensaje = "Se ha creado exitosamente su cuenta de Kafen como vendedor, ya puede empezar a vender";
                                string asunto = "Se creo su cuenta de Kanfe";
                                ServicioEmail EnvioCorreo = new ServicioEmail();
                                EnvioCorreo.EnviarCorreo(oUsuario.Correo_Electronico.ToString(), mensaje, asunto);
                            }
                            catch (Microsoft.Data.SqlClient.SqlException)
                            {
                                Duplicado = true;

                                return RedirectToAction("Index", "Vendedor");
                            }

                        }


                        return RedirectToAction("Index", "Home", new RouteValueDictionary(new { Registrado = true }));

                    }
                }

            }
            else
            {
                return RedirectToAction("Index", "Vendedor");

            }
        }


        [Authorize(Roles = "2")]

        public IActionResult Pedidos()
        {
            if (tope)
            {
                ViewData["tope"] = "El pedido ya tiene el estatus maximo!";

            }
            if (succesfull)
            {
                ViewData["si"] = "El estatus del pedido se cambio satisfactoriamente!";

            }
            tope = false;
            succesfull = false;

            var model = _repositorioPedidos.ListarTodos().Select(a => new PedidosViewModel
            {

                id = a.Id,
                Cliente = a.Cliente.Nombre,
                ApMaterno = a.Cliente.Apellido_Materno,
                ApPaterno = a.Cliente.Apellido_Paterno,
                Total = a.Total,
                Estatus = a.Estatus.descripcion,
                Fecha = a.Fecha.ToString("d")



            }
         );
            return View(model);



        }

        [Authorize(Roles = "2")]
        public IActionResult DetalleVenta(Guid id)
        {
            var model = _repositorioDetalleVenta.ListarTodos().Where(a => a.IdPedido.Id == id).Select(a => new DetalleVentaViewModel
            {

                IdPedido = a.Id,
                Producto = a.IdProducto.Nombre,
                Cantidad = a.Cantidad,
                Precio = a.Precio




            }
          );
            return View(model);
        }

        [Authorize(Roles = "2")]
        public IActionResult CambiarEstatus(Guid id)
        {
            using (SqlConnection cm = new SqlConnection(cadena))
            {
                SqlCommand cnd = new SqlCommand("select EstatusId from Pedidos where Id = @id", cm);
                cnd.Parameters.AddWithValue("@id", id);
                cnd.CommandType = CommandType.Text;
                cm.Open();
                SqlDataReader Rleido;
                Rleido = cnd.ExecuteReader();
                int filas = Convert.ToInt32(Rleido.HasRows);
                if (Rleido.Read())
                {
                    if (Convert.ToInt32(Rleido[0]) != 4 && Convert.ToInt32(Rleido[0]) != 5)
                    {
                        using (SqlConnection ky = new SqlConnection(cadena))
                        {
                            SqlCommand kd = new SqlCommand("update Pedidos Set EstatusId = EstatusId+1 where Id = @id ", ky);
                            {
                                kd.Parameters.AddWithValue("@id", id);
                                kd.CommandType = CommandType.Text;
                                ky.Open();
                                kd.ExecuteNonQuery();

                                ky.Close();
                                succesfull = true;
                                return RedirectToAction("Pedidos", "Vendedor");

                            }
                        }
                    }
                    else
                    {
                        tope = true;
                        return RedirectToAction("Pedidos", "Vendedor");


                    }




                }
                else
                {
                    return RedirectToAction("Pedidos", "Vendedor");

                }


            }


        }
        [Authorize(Roles = "2")]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return (RedirectToAction("Index", "Home"));
        }
        [Authorize(Roles = "2")]
        public IActionResult Estadisticas()
        {
            return View();
        }
        [Authorize(Roles = "2")]
        public JsonResult DataPastel()
        {
            SeriePastel serie = new SeriePastel();
            return Json(serie.GetDataDummy());
        }
        [Authorize(Roles = "2")]
        public JsonResult DataBarras()
        {
            SerieBarras serie = new SerieBarras();
            return Json(serie.GetDataDummy());
        }

    }
}
