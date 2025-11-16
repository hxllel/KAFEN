using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafen.Dominio.dominio
{
    public abstract class Entidad<TId> : IEquatable<Entidad<TId>>
    {
        public TId Id { get; protected set; }
        protected Entidad(TId id)
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentNullException(
                    "No se puede agregar el valor por defecto al id", "id");
            }
            this.Id = id;
        }
        public override bool Equals(object obj)
        {
            var Entity = obj as Entidad<TId>;
            if (Entity != null)
            {
                return this.Equals(Entity);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public bool Equals(Entidad<TId> other)
        {
            if (other == null)
            {
                return false;
            }
            return Id.Equals(other.Id);
        }
    }
}
