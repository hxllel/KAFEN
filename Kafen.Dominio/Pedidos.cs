using Kafen.Dominio.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Dominio
{
    public class Pedido : Entidad<Guid>
    {
        public Pedido(Guid id) : base(id)
        {
            Id = id;
        }
        public Usuario Cliente { get; protected set; }
        public decimal Total { get; protected set; }
        public Estatus Estatus { get; protected set; }

        public DateTime Fecha { get; protected set; }


        public static Pedido Crear(Guid id, Usuario cliente,
            int total, Estatus estatus, DateTime fecha)
        {
            Pedido pedido = new Pedido(id);
            pedido.Actualizar(cliente, total, estatus, fecha);
            return pedido;
        }
        public void Actualizar(Usuario cliente, decimal total, Estatus estatus, DateTime fecha)
        {

            Cliente = cliente;
            Total = total;
            Estatus = estatus;
            Fecha = fecha;
        }
    }
}
