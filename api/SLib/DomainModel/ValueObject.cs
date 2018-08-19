namespace SLib.DomainModel
{
    public abstract class ValueObject<TId>
    {
        public TId Id {get; protected set;}

        /// <summary>
        ///   Determines whether this instance has an id.
        ///   If the Id has a default value for its type then it is considered not to have an assigned Id.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has id; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool HasId()
        {
            return !Equals(Id, default(TId));
        }
    }
}
