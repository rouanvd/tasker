using System;

namespace SLib.DomainModel.Auditing
{
    /// <summary>
    ///   An interface for all domain objects that should take part in ADD & UPDATE auditing.
    ///   
    ///   This interface also allows the infrastructure to automatically populate
    ///   the audit fields when persisting objects.
    /// </summary>
    public interface IAddUpdateAuditable
    {
        string CreatedBy {get;set;}
        string CreatedByFullName {get;set;}
        DateTime? CreatedOn {get;set;}

        string UpdatedBy {get;set;}
        string UpdatedByFullName {get;set;}
        DateTime? UpdatedOn {get;set;}
    }
}
