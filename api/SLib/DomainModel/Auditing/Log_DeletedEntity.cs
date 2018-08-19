using System;

namespace SLib.DomainModel.Auditing
{
    public class Log_DeletedEntity : Entity<long>
    {
        public override long Id {get;set;}

        /// <summary>
        ///   The FULLY-QUALIFIED type of the deleted entity.
        /// </summary>
        public string EntityType {get;set;}

        /// <summary>
        ///   Any additional information that might need to be stored about the deleted entity.
        ///   This should be structured content (such as JSON / XML) so we can easily parse it.
        /// </summary>
        public string EntityInfo {get;set;}

        public string DeletedBy {get;set;}
        public string DeletedByFullName {get;set;}
        public DateTime DeletedOn {get;set;}
    }
}
