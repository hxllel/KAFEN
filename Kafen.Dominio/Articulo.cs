using Kafen.Dominio.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Dominio
{
    public class Articulo : Entidad<Guid>
    {
        public Articulo(Guid id) : base(id)
        {
            Id = id;
        }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        [Required]
        public int Precio { get; protected set; }
        [Required]
        public Categoria Categoria { get; protected set; }
        public void Actualizar(string nombre, string descripcion, int precio, Categoria categoria)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                throw new ArgumentException("El nombre es requerido");
            }

            if (string.IsNullOrEmpty(descripcion))
            {
                throw new ArgumentException("La descripcion es requerida");
            }
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            Categoria = categoria;
        }
        public static Articulo Crear(Guid id, string nombre, string descripcion,
            int precio, Categoria categoria)
        {
            Articulo articulo = new Articulo(id);
            articulo.Actualizar(nombre, descripcion, precio, categoria);
            return articulo;
        }
    }
}
