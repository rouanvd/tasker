namespace SLib.DomainModel.Auditing
{
    /// <summary>
    ///   An interface for all domain objects that should take part in DELETE auditing.
    ///   
    ///   This interface also allows the infrastructure to automatically populate
    ///   the Delete Log when persisting deleted objects.
    /// </summary>
    public interface IDeleteAuditable
    {
        string GetExtraInfoForDeleteAuditing();
    }
}
