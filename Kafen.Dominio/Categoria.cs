using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Dominio
{
    public class Categoria
    {
        public Categoria(int id)
        {
            Id = id;
        }

        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public void Actualizar(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new ArgumentException("El nombre es requerido");


            Nombre = nombre;
        }

        public static Categoria Crear(int id, string nombre,
            string email)
        {
            Categoria categoria = new Categoria(id);
            categoria.Actualizar(nombre);
            return categoria;
        }


    }
}
