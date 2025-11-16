using Kafen.Datos;
using Kafen.Datos.Repositorios;
using Kafen.Dominio;
using Kafen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Linq;

namespace Kafen.Controllers
{
    [Authorize(Roles = "2")]
    public class ArticuloController : Controller
    {
        static string cadena = "workstation id=KafenData.mssql.somee.com;packet size=4096;user id=Kafen_SQLLogin_1;pwd=u1nxeltc55;data source=KafenData.mssql.somee.com;persist security info=False;initial catalog=KafenData";

        private readonly IRepositorioArticulo _repositorioArticulo;
        private readonly IRepositorioCategoria _repositorioCategoria;
        public ArticuloController(IRepositorioArticulo repositorioArticulo,
            IRepositorioCategoria repositorioCategoria)
        {
            _repositorioArticulo = repositorioArticulo;
            _repositorioCategoria = repositorioCategoria;
        }
        public IActionResult Index()
        {
            //var articulos = _repositorioArticulo.ListarTodos();
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

        [HttpGet]
        public IActionResult Editar(Guid id)
        {
            var articulo = _repositorioArticulo.BuscarporId(id);
            var model = new ArticuloViewModel
            {
                Id = id,
                Nombre = articulo.Nombre,
                Descripcion = articulo.Descripcion,
                Precio = articulo.Precio
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Editar(ArticuloEntrada model)
        {
            var articulo = new Articulo(model.Id);
            var Categoria = _repositorioCategoria.BuscarPorId(model.Categoria);
            articulo.Actualizar(model.Nombre, model.Descripcion, model.Precio, Categoria);
            _repositorioArticulo.Modificar(articulo);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Crear()
        {
            var model = new ArticuloViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Crear(ArticuloEntrada model)
        {
            if (ModelState.IsValid)
            {
                var articulo = new Articulo(Guid.NewGuid());
                var Categoria = _repositorioCategoria.BuscarPorId(model.Categoria);
                articulo.Actualizar(model.Nombre, model.Descripcion, model.Precio, Categoria);
                _repositorioArticulo.Agregar(articulo);
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Eliminar(Guid id)
        {
            var articulo = _repositorioArticulo.BuscarporId(id);
            var model = new ArticuloViewModel
            {
                Id = articulo.Id,
                Nombre = articulo.Nombre,
                Descripcion = articulo.Descripcion,
                Precio = articulo.Precio,
                Categoria = articulo.Categoria.Nombre
            };
            return View(model);
        }
        [HttpPost, ActionName("Eliminar")]
        public ActionResult EliminarConfirmado(Guid id)
        {

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Delete from Carrito where IdArticuloId = @id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();

                cn.Close();

            }
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("Delete from Detalleventa where IdProductoId = @id", cn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.Text;
                cn.Open();
                cmd.ExecuteNonQuery();

                cn.Close();

            }
            _repositorioArticulo.Eliminar(id);
            return RedirectToAction("Index");
        }

        public IActionResult Detalle(Guid id)
        {
            var articulo = _repositorioArticulo.BuscarporId(id);
            var model = new ArticuloViewModel
            {
                Id = articulo.Id,
                Nombre = articulo.Nombre,
                Descripcion = articulo.Descripcion,
                Precio = articulo.Precio,
                Categoria = articulo.Categoria.Nombre

            };
            return View(model);
        }




    }
}
