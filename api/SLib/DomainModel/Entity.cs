using System;

namespace SLib.DomainModel
{
    /// <summary>
    ///   This is the base class for a DDD Entity.  All entities should inherit from this class.
    /// </summary>
    /// <typeparam name="TId">Type of the Id property of the Entity.</typeparam>
    public abstract class Entity<TId>
    {
        /// <summary>
        ///   Every entity has an Identity which allows us to track the entity throughout the system even if other identifying
        ///   properties change throughout the lifetime of the entity.
        /// </summary>
        public abstract TId Id {get;set;}

        /// <summary>
        ///   Used by Entity Framework for optimistic concurrency.
        /// </summary>
        public Byte[] ChangeTimestamp {get;set;}


        public bool HasId()
        {
            TId typeDefaultValue = default(TId);
            return ! Equals(Id, typeDefaultValue);
        }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Entity<TId>)obj);
        }


        public override int GetHashCode()
        {
            return (HasId() ? Id.GetHashCode() : 0);
        }


        protected bool Equals(Entity<TId> other)
        {
            return Equals(Id, other.Id);
        }
    }
}
