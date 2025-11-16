using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Kafen.Models
{
    public class SeriePastel
    {
        static string cadena = "workstation id=KafenData.mssql.somee.com;packet size=4096;user id=Kafen_SQLLogin_1;pwd=u1nxeltc55;data source=KafenData.mssql.somee.com;persist security info=False;initial catalog=KafenData";
        static int Frituras = 0, Comida = 0, Bebidas = 0;

        public string name { get; set; }
        public double y { get; set; }
        public bool sliced { get; set; }
        public bool selected { get; set; }


        public SeriePastel()
        {

        }
        public SeriePastel(string name, double y, bool sliced = false, bool selected = false)
        {
            this.name = name;
            this.y = y;
            this.sliced = sliced;
            this.selected = selected;
        }

        public List<SeriePastel> GetDataDummy()
        {
            List<SeriePastel> Lista = new List<SeriePastel>();

            using (SqlConnection cm = new SqlConnection(cadena))
            {
                SqlCommand cnd = new SqlCommand("select count (ca.Nombre) from Detalleventa de Join Articulos ar on ar.Id = de.IdProductoId Join Categoria ca on ar.CategoriaId = ca.Id Join Pedidos pe on pe.Id = de.IdPedidoId where ca.Nombre = 'Comida'and pe.EstatusId = '4'", cm);
                cnd.CommandType = CommandType.Text;
                cm.Open();
                SqlDataReader Rleido;
                Rleido = cnd.ExecuteReader();
                int filas = Convert.ToInt32(Rleido.HasRows);
                if (Rleido.Read())
                {
                    Comida = Convert.ToInt32(Rleido[0]);
                }
                else
                {
                    Comida = 0;
                }
            }

            using (SqlConnection cm = new SqlConnection(cadena))
            {
                SqlCommand cnd = new SqlCommand("select count(ca.Nombre) from Detalleventa de Join Articulos ar on ar.Id = de.IdProductoId Join Categoria ca on ar.CategoriaId = ca.Id Join Pedidos pe on pe.Id = de.IdPedidoId where ca.Nombre = 'Frituras'and pe.EstatusId = '4'", cm);
                cnd.CommandType = CommandType.Text;
                cm.Open();
                SqlDataReader Rleido;
                Rleido = cnd.ExecuteReader();
                int filas = Convert.ToInt32(Rleido.HasRows);
                if (Rleido.Read())
                {
                    Frituras = Convert.ToInt32(Rleido[0]);
                }
                else
                {
                    Frituras = 0;
                }
            }

            using (SqlConnection cm = new SqlConnection(cadena))
            {
                SqlCommand cnd = new SqlCommand("select count (ca.Nombre) from Detalleventa de Join Articulos ar on ar.Id = de.IdProductoId Join Categoria ca on ar.CategoriaId = ca.Id Join Pedidos pe on pe.Id = de.IdPedidoId where ca.Nombre = 'Bebidas'and pe.EstatusId = '4'", cm);
                cnd.CommandType = CommandType.Text;
                cm.Open();
                SqlDataReader Rleido;
                Rleido = cnd.ExecuteReader();
                int filas = Convert.ToInt32(Rleido.HasRows);
                if (Rleido.Read())
                {
                    Bebidas = Convert.ToInt32(Rleido[0]);
                }
                else
                {
                    Bebidas = 0;
                }
            }


            Lista.Add(new SeriePastel("Frituras", Frituras));
            Lista.Add(new SeriePastel("Comida", Comida));
            Lista.Add(new SeriePastel("Bebidas", Bebidas));


            return Lista;
        }

    }
}
