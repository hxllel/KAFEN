using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace Kafen.Models
{
    public class SerieBarras
    {

        static string primero, segundo, tercero, cuarto, quinto;
        static public int valorprimero = 0, valorsegundo = 0, valortercero = 0, valorcuarto = 0, valorquinto = 0;
        static int aver1, aver2, aver3, aver4, aver5;



        static string cadena = "workstation id=KafenData.mssql.somee.com;packet size=4096;user id=Kafen_SQLLogin_1;pwd=u1nxeltc55;data source=KafenData.mssql.somee.com;persist security info=False;initial catalog=KafenData";

        public SerieBarras()
        {

        }

        public object[] GetDataDummy()
        {
            string[] fechas =
           {
            "05/05/2105", "05/05/2105", "05/05/2105", "05/05/2105" , "05/05/2105"
        };
            int[] valores =
          {
            0,0,0,0,0
        };
            object[] data = new object[5];


            for (int i = 0; i < 5; i++)
            {
                using (SqlConnection cm = new SqlConnection(cadena))
                {
                    SqlCommand cnd = new SqlCommand("select distinct Fecha from Pedidos where Fecha != @aux1 and Fecha != @aux2 and Fecha != @aux3 and Fecha != @aux4 and Fecha != @aux5 order by Fecha desc", cm);
                    cnd.CommandType = CommandType.Text;
                    cnd.Parameters.AddWithValue("@aux1", Convert.ToDateTime(fechas[0]));
                    cnd.Parameters.AddWithValue("@aux2", Convert.ToDateTime(fechas[1]));
                    cnd.Parameters.AddWithValue("@aux3", Convert.ToDateTime(fechas[2]));
                    cnd.Parameters.AddWithValue("@aux4", Convert.ToDateTime(fechas[3]));
                    cnd.Parameters.AddWithValue("@aux5", Convert.ToDateTime(fechas[4]));



                    cm.Open();
                    SqlDataReader Rleido;
                    Rleido = cnd.ExecuteReader();
                    int filas = Convert.ToInt32(Rleido.HasRows);
                    if (Rleido.Read())
                    {
                        if (fechas[0] == "05/05/2105")
                        {
                            fechas[0] = Convert.ToString(Rleido[0]);
                        }
                        else if (fechas[1] == "05/05/2105")
                        {
                            fechas[1] = Convert.ToString(Rleido[0]);
                        }
                        else if (fechas[2] == "05/05/2105")
                        {
                            fechas[2] = Convert.ToString(Rleido[0]);
                        }
                        else if (fechas[3] == "05/05/2105")
                        {
                            fechas[3] = Convert.ToString(Rleido[0]);
                        }
                        else if (fechas[4] == "05/05/2105")
                        {
                            fechas[4] = Convert.ToString(Rleido[0]);
                        }
                    }

                    else
                    {
                        if (fechas[0] == "05/05/2105")
                        {
                            primero = "No existe registro para esta fecha";
                            valorprimero = 0;
                        }
                        if (fechas[1] == "05/05/2105")
                        {
                            segundo = "No existe registro para esta fecha";
                            valorsegundo = 0;

                        }
                        if (fechas[2] == "05/05/2105")
                        {
                            tercero = "No existe registro para esta fecha";
                            valortercero = 0;

                        }
                        if (fechas[3] == "05/05/2105")
                        {
                            cuarto = "No existe registro para esta fecha";
                            valorcuarto = 0;

                        }
                        if (fechas[4] == "05/05/2105")
                        {
                            quinto = "No existe registro para esta fecha";
                            valorquinto = 0;

                        }
                    }
                }
            }

            if (primero != "No existe registro para esta fecha")
            {
                using (SqlConnection cm = new SqlConnection(cadena))
                {
                    SqlCommand cnd = new SqlCommand("select count (Id) from Pedidos where Fecha = @aux1 and EstatusId = '4'", cm);
                    cnd.CommandType = CommandType.Text;
                    cnd.Parameters.AddWithValue("@aux1", Convert.ToDateTime(fechas[0]));
                    cm.Open();
                    SqlDataReader Rleido;
                    Rleido = cnd.ExecuteReader();
                    int filas = Convert.ToInt32(Rleido.HasRows);
                    if (Rleido.Read())
                    {
                        valores[0] = Convert.ToInt32(Rleido[0]);
                        aver1 = Convert.ToInt32(Rleido[0]);
                    }
                }
            }
            else
            {
                fechas[0] = "No existe registro para esta fecha";
                valores[0] = 0;
            }

            if (segundo != "No existe registro para esta fecha")
            {
                using (SqlConnection cm = new SqlConnection(cadena))
                {
                    SqlCommand cnd = new SqlCommand("select count (Id) from Pedidos where Fecha = @aux1 and EstatusId = '4'", cm);
                    cnd.CommandType = CommandType.Text;
                    cnd.Parameters.AddWithValue("@aux1", Convert.ToDateTime(fechas[1]));
                    cm.Open();
                    SqlDataReader Rleido;
                    Rleido = cnd.ExecuteReader();
                    int filas = Convert.ToInt32(Rleido.HasRows);
                    if (Rleido.Read())
                    {
                        valores[1] = Convert.ToInt32(Rleido[0]);
                        aver2 = Convert.ToInt32(Rleido[0]);

                    }
                }
            }
            else
            {
                fechas[1] = "No existe registro para esta fecha";
                valores[1] = 0;
            }

            if (tercero != "No existe registro para esta fecha")
            {
                using (SqlConnection cm = new SqlConnection(cadena))
                {
                    SqlCommand cnd = new SqlCommand("select count (Id) from Pedidos where Fecha = @aux1 and EstatusId = '4'", cm);
                    cnd.CommandType = CommandType.Text;
                    cnd.Parameters.AddWithValue("@aux1", Convert.ToDateTime(fechas[2]));
                    cm.Open();
                    SqlDataReader Rleido;
                    Rleido = cnd.ExecuteReader();
                    int filas = Convert.ToInt32(Rleido.HasRows);
                    if (Rleido.Read())
                    {
                        valores[2] = Convert.ToInt32(Rleido[0]);
                        aver3 = Convert.ToInt32(Rleido[0]);

                    }
                }
            }
            else
            {
                fechas[2] = "No existe registro para esta fecha";
                valores[2] = 0;
            }

            if (cuarto != "No existe registro para esta fecha")
            {
                using (SqlConnection cm = new SqlConnection(cadena))
                {
                    SqlCommand cnd = new SqlCommand("select count (Id) from Pedidos where Fecha = @aux1 and EstatusId = '4'", cm);
                    cnd.CommandType = CommandType.Text;
                    cnd.Parameters.AddWithValue("@aux1", Convert.ToDateTime(fechas[3]));
                    cm.Open();
                    SqlDataReader Rleido;
                    Rleido = cnd.ExecuteReader();
                    int filas = Convert.ToInt32(Rleido.HasRows);
                    if (Rleido.Read())
                    {
                        valores[3] = Convert.ToInt32(Rleido[0]);
                        aver4 = Convert.ToInt32(Rleido[0]);

                    }
                }
            }
            else
            {
                fechas[3] = "No existe registro para esta fecha";
                valores[3] = 0;
            }

            if (quinto != "No existe registro para esta fecha")
            {
                using (SqlConnection cm = new SqlConnection(cadena))
                {
                    SqlCommand cnd = new SqlCommand("select count (Id) from Pedidos where Fecha = @aux1 and EstatusId = '4'", cm);
                    cnd.CommandType = CommandType.Text;
                    cnd.Parameters.AddWithValue("@aux1", Convert.ToDateTime(fechas[4]));
                    cm.Open();
                    SqlDataReader Rleido;
                    Rleido = cnd.ExecuteReader();
                    int filas = Convert.ToInt32(Rleido.HasRows);
                    if (Rleido.Read())
                    {
                        valores[4] = Convert.ToInt32(Rleido[0]);
                        aver5 = Convert.ToInt32(Rleido[0]);

                    }
                }
            }
            else
            {
                fechas[4] = "No existe registro para esta fecha";
                valores[4] = 0;
            }


            for (int i = 0; i < valores.Length; i++)
            {
                for (int j = 0; j < valores.Length - 1; j++)
                {
                    if (valores[j] > valores[j + 1])
                    {
                        int aux = valores[j];
                        string aux2 = fechas[j];
                        valores[j] = valores[j + 1];
                        fechas[j] = fechas[j + 1];
                        valores[j + 1] = aux;
                        fechas[j + 1] = aux2;
                    }
                }
            }





            data[0] = new object[] { fechas[4], valores[4] };
            data[1] = new object[] { fechas[3], valores[3] };
            data[2] = new object[] { fechas[2], valores[2] };
            data[3] = new object[] { fechas[1], valores[1] };
            data[4] = new object[] { fechas[0], valores[0] };

            return data;
        }



    }
}
