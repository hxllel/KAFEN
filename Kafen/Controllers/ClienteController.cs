using Kafen.Datos;
using Kafen.Datos.Repositorios;
using Kafen.Dominio;
using Kafen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Kafen.Controllers
{
    [Authorize(Roles = "1")]
    public class ClienteController : Controller
    {
        static bool cancelado = false;
        static bool nosepuede = false;
        static bool agregado = false;
        static bool noagregado = false;
        static bool Pedido = false;
        static bool carritovacio = false;
        static string cadena = "workstation id=KafenData.mssql.somee.com;packet size=4096;user id=Kafen_SQLLogin_1;pwd=u1nxeltc55;data source=KafenData.mssql.somee.com;persist security info=False;initial catalog=KafenData";
        static public int idusuario = 0;
        private readonly IRepositorioArticulo _repositorioArticulo;
        private readonly IRepositorioCategoria _repositorioCategoria;
        private readonly IRepositorioCarrito _repositorioCarrito;
        private readonly IRepositorioUsuario _repositorioUsuario;
        private readonly IRepositorioPedidos _repositorioPedidos;
        private readonly IRepositorioEstatus _repositorioEstatus;
        private readonly IRepositorioDetalleVenta _repositorioDetalleVenta;
        public ClienteController(IRepositorioArticulo repositorioArticulo,
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


        [HttpGet]
        public IActionResult Tienda(int usuario, int idcategoria = 0)
        {
            if (carritovacio)
            {
                ViewData["Carrito"] = "No se puede realizar el pedido habiendo ningun articulo en el carrito";

            }
            if (agregado)
            {
                ViewData["Agregado"] = "Se ha agregado el producto al carrito";
            }
            if (Pedido)
            {
                ViewData["Pedido"] = "Su pedido ya ha sido de alta";
            }

            if (noagregado)
            {
                ViewData["NoAgregado"] = "El producto ya esta en el carrito";

            }
            carritovacio = false;
            agregado = false;
            noagregado = false;
            Pedido = false;

            if (idusuario != 0)
            {
                usuario = idusuario;
            }

            if (usuario == 0)
            {

                RedirectToAction("Index", "Home");
            }
            else if (usuario != 0)
            {

                idusuario = usuario;
                if (idcategoria == 0)
                {
                    var model = _repositorioArticulo.ListarTodos().Select(a => new ArticuloViewModel
                    {
                        Id = a.Id,
                        Nombre = a.Nombre,
                        Descripcion = a.Descripcion,
                        Precio = a.Precio,
                        Categoria = a.Categoria.Nombre

                    });
                    return View(model);
                }

                else if (idcategoria != 0)
                {
                    var categoria = _repositorioCategoria.BuscarPorId(idcategoria);
                    var model = _repositorioArticulo.ListarTodos().Where(a => a.Categoria.Id == idcategoria).Select(a => new ArticuloViewModel
                    {

                        Id = a.Id,
                        Nombre = a.Nombre,
                        Descripcion = a.Descripcion,
                        Precio = a.Precio,
                        Categoria = a.Categoria.Nombre
                    }

                    );
                    return View(model);
                }
            }
            return (RedirectToAction("Index", "Home"));

        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            idusuario = 0;
            return (RedirectToAction("Index", "Home"));
        }


        public IActionResult AgregarAlCarrito(Guid id, CarritoViewModel model)
        {
            using (SqlConnection oConexion = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("ValidarProducto", oConexion);
                cmd.Parameters.AddWithValue("@idusuario", idusuario);
                cmd.Parameters.AddWithValue("@idProducto", id);


                cmd.CommandType = CommandType.StoredProcedure;

                oConexion.Open();

                // cmd.ExecuteNonQuery();
                SqlDataReader Rleido;
                Rleido = cmd.ExecuteReader();
                int filas = Convert.ToInt32(Rleido.HasRows);
                //   {
                if (Rleido.Read())
                {
                    noagregado = true;
                    return RedirectToAction("Tienda", "Cliente");


                }

                //  }
                else
                {
                    var carrito = new Carrito(Guid.NewGuid());
                    var Articulo = _repositorioArticulo.BuscarporId(id);
                    var usuario = _repositorioUsuario.BuscarporId(idusuario);
                    var cantidad = 1;
                    carrito.Actualizar(Articulo, usuario, cantidad);
                    _repositorioCarrito.Agregar(carrito);
                    agregado = true;
                    return RedirectToAction("Tienda", "Cliente");
                }

            }

            // return RedirectToAction("Index", "Home");
        }




        public IActionResult Carrito()
        {
            var model = _repositorioCarrito.ListarTodos().Where(a => a.IdUsuario.Id == idusuario).Select(a => new CarritoVista
            {

                Id = a.Id,
                Producto = a.IdArticulo.Nombre,
                Descripcion = a.IdArticulo.Descripcion,
                Precio = a.IdArticulo.Precio,
                Cantidad = a.Cantidad


            }
           );
            return View(model);
        }


        public IActionResult EliminarArticulo(Guid id)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Delete from Carrito where Id = @id and IdUsuarioId = @idusuario ", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@idusuario", idusuario);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();

                cn.Close();

            }
            return RedirectToAction("Carrito", "Cliente");
        }


        public IActionResult AgregarCantidad(Guid id)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("update Carrito Set Cantidad = Cantidad+1 where Id = @id and IdUsuarioId = @idusuario ", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@idusuario", idusuario);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();

                cn.Close();

            }

            return RedirectToAction("Carrito", "Cliente");
        }
        public IActionResult QuitarCantidad(Guid id)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("update Carrito Set Cantidad = Cantidad-1 where Id = @id and IdUsuarioId = @idusuario  ", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@idusuario", idusuario);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();

                cn.Close();

            }

            return RedirectToAction("Carrito", "Cliente");
        }






        public IActionResult AgregarPedido(CarritoVista model)
        {

            int cantidad = 0;
            int productos = 0;
            string articulo;
            int numero = 0;
            int multi = 0;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("select sum(ar.Precio*ca.Cantidad) from Carrito ca Join Articulos ar on ca.IdArticuloId = ar.Id where IdUsuarioId = @idusuario", cn);
                cmd.Parameters.AddWithValue("@idusuario", idusuario);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                SqlDataReader Rleido;
                Rleido = cmd.ExecuteReader();
                int filas = Convert.ToInt32(Rleido.HasRows);
                try
                {
                    if (Rleido.Read())
                    {
                        cantidad = Convert.ToInt32(Rleido[0]);
                    }
                }
                catch (InvalidCastException)
                {
                    carritovacio = true;
                    return RedirectToAction("Tienda", "Cliente");

                }



            }

            var pedido = new Pedido(Guid.NewGuid());

            var estatus = _repositorioEstatus.BuscarporId(1);
            var usuario = _repositorioUsuario.BuscarporId(idusuario);
            var Fecha = DateTime.Today;
            var Total = cantidad;

            pedido.Actualizar(usuario, Total, estatus, Fecha);
            _repositorioPedidos.Agregar(pedido);


            using (SqlConnection cm = new SqlConnection(cadena))
            {
                SqlCommand cnd = new SqlCommand("select count (Id) from carrito where IdUsuarioId = @idusuario", cm);
                cnd.Parameters.AddWithValue("@idusuario", idusuario);
                cnd.CommandType = CommandType.Text;
                cm.Open();
                SqlDataReader Rleido;
                Rleido = cnd.ExecuteReader();
                int filas = Convert.ToInt32(Rleido.HasRows);
                if (Rleido.Read())
                {
                    productos = Convert.ToInt32(Rleido[0]);
                }

            }

            for (int i = 0; i < productos; i++)
            {
                using (SqlConnection cm = new SqlConnection(cadena))
                {
                    SqlCommand cnd = new SqlCommand("select * from Carrito where IdUsuarioId = @idusuario", cm);
                    cnd.Parameters.AddWithValue("@idusuario", idusuario);
                    cnd.CommandType = CommandType.Text;
                    cm.Open();
                    SqlDataReader Rleido;
                    Rleido = cnd.ExecuteReader();
                    int filas = Convert.ToInt32(Rleido.HasRows);
                    if (Rleido.Read())
                    {
                        articulo = Convert.ToString(Rleido[1]);

                        numero = Convert.ToInt32(Rleido[3]);

                        using (SqlConnection jc = new SqlConnection(cadena))
                        {
                            SqlCommand cj = new SqlCommand("select ar.Precio*ca.Cantidad from Carrito ca Join Articulos ar on ca.IdArticuloId = ar.Id where IdUsuarioId = @idusuario", jc);
                            cj.Parameters.AddWithValue("@idusuario", idusuario);
                            cj.CommandType = CommandType.Text;
                            jc.Open();
                            SqlDataReader rleido;
                            rleido = cj.ExecuteReader();
                            int filas2 = Convert.ToInt32(rleido.HasRows);
                            if (rleido.Read())
                            {
                                multi = Convert.ToInt32(rleido[0]);


                                var idetalle = new DetalleVenta(Guid.NewGuid());
                                var NUMERO = numero;
                                // var PEDIDO = _repositorioPedidos.BuscarporId(Guid.Parse(pedido.ToString()));
                                var MULTI = multi;
                                var idarticulo = _repositorioArticulo.BuscarporId(Guid.Parse(articulo));


                                idetalle.Actualizar(pedido, idarticulo, NUMERO, MULTI);

                                _repositorioDetalleVenta.Agregar(idetalle);



                                using (SqlConnection ky = new SqlConnection(cadena))
                                {
                                    SqlCommand em = new SqlCommand("delete Carrito where IdUsuarioId = @id and IdArticuloId = @idarticulo  ", ky);
                                    em.Parameters.AddWithValue("@id", idusuario);
                                    em.Parameters.AddWithValue("@idarticulo", articulo);
                                    em.CommandType = CommandType.Text;
                                    ky.Open();
                                    em.ExecuteNonQuery();

                                    ky.Close();

                                }
                            }


                        }
                    }

                }
            }
            Pedido = true;

            return RedirectToAction("Tienda", "Cliente");

        }


        public IActionResult PedidosEnCurso()
        {
            if (cancelado)
            {
                ViewData["cancelado"] = "Se ha cancelado satisfactoriamente el pedido!";

            }
            if (nosepuede)
            {
                ViewData["nosepuede"] = "El pedido esta en un punto en el cual ya no puede ser cancelado!";

            }
            cancelado = false;
            nosepuede = false;

            var model = _repositorioPedidos.ListarTodos().Where(a => a.Estatus.Id != 4 && a.Cliente.Id == idusuario && a.Estatus.Id != 5).Select(a => new PedidosViewModel
            {

                id = a.Id,
                Total = a.Total,
                Estatus = a.Estatus.descripcion,
                Fecha = a.Fecha.ToString("d")



            }
          );
            return View(model);
        }

        public IActionResult DetalleDeVenta(Guid id)
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

        public JsonResult ObtenerCarrito()
        {
            int cantidad;
            using (SqlConnection ky = new SqlConnection(cadena))
            {
                SqlCommand cj = new SqlCommand("select count(id) from Carrito where Id = @idusuario", ky);
                cj.Parameters.AddWithValue("@idusuario", idusuario);
                cj.CommandType = CommandType.Text;
                ky.Open();
                SqlDataReader rleido;
                rleido = cj.ExecuteReader();
                int filas2 = Convert.ToInt32(rleido.HasRows);
                if (rleido.Read())
                {
                    cantidad = Convert.ToInt32(rleido[0]);
                }
                else
                {
                    cantidad = 0;
                }
            }

            return Json(cantidad);
        }




        public IActionResult Historial()
        {
            var model = _repositorioPedidos.ListarTodos().Where(a => a.Estatus.Id == 4 && a.Cliente.Id == idusuario || a.Estatus.Id == 5 && a.Cliente.Id == idusuario).Select(a => new PedidosViewModel
            {

                id = a.Id,
                Total = a.Total,
                Estatus = a.Estatus.descripcion,
                Fecha = a.Fecha.ToString("d")



            }
          );
            return View(model);
        }




        public IActionResult AgregarProducto2(Guid Id)
        {
            string idproducto;

            using (SqlConnection ky = new SqlConnection(cadena))
            {

                SqlCommand cj = new SqlCommand("select IdProductoId from DetalleVenta where Id = @idusuario", ky);
                cj.Parameters.AddWithValue("@idusuario", Id);
                cj.CommandType = CommandType.Text;
                ky.Open();
                SqlDataReader rleido;
                rleido = cj.ExecuteReader();
                int filas2 = Convert.ToInt32(rleido.HasRows);
                if (rleido.Read())
                {
                    idproducto = Convert.ToString(rleido[0]);


                    using (SqlConnection oConexion = new SqlConnection(cadena))
                    {

                        SqlCommand cmd = new SqlCommand("ValidarProducto", oConexion);
                        cmd.Parameters.AddWithValue("@idusuario", idusuario);
                        cmd.Parameters.AddWithValue("@idProducto", Guid.Parse(idproducto));


                        cmd.CommandType = CommandType.StoredProcedure;

                        oConexion.Open();

                        // cmd.ExecuteNonQuery();
                        SqlDataReader Rleido;
                        Rleido = cmd.ExecuteReader();
                        int filas = Convert.ToInt32(Rleido.HasRows);
                        //   {
                        if (Rleido.Read())
                        {
                            noagregado = true;
                            return RedirectToAction("Tienda", "Cliente");


                        }

                        //  }
                        else
                        {
                            var carrito = new Carrito(Guid.NewGuid());
                            var Articulo = _repositorioArticulo.BuscarporId(Guid.Parse(idproducto));
                            var usuario = _repositorioUsuario.BuscarporId(idusuario);
                            var cantidad = 1;
                            carrito.Actualizar(Articulo, usuario, cantidad);
                            _repositorioCarrito.Agregar(carrito);
                            agregado = true;
                            return RedirectToAction("Tienda", "Cliente");
                        }

                    }


                }




            }

            return View();
        }


        public IActionResult CancelarPedido(Guid id)
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
                    if (Convert.ToInt32(Rleido[0]) > 2)
                    {
                        nosepuede = true;
                        return RedirectToAction("PedidosEnCurso", "Cliente");
                    }
                    else
                    {
                        using (SqlConnection ky = new SqlConnection(cadena))
                        {
                            SqlCommand kd = new SqlCommand("update Pedidos Set EstatusId = 5 where Id = @id ", ky);
                            {
                                kd.Parameters.AddWithValue("@id", id);
                                kd.CommandType = CommandType.Text;
                                ky.Open();
                                kd.ExecuteNonQuery();

                                ky.Close();
                                cancelado = true;
                                return RedirectToAction("PedidosEnCurso", "Cliente");

                            }
                        }
                    }



                }
            }
            return View();
        }




    }
}
