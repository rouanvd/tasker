using System.Collections.Generic;

namespace SLib.DomainModel
{
    public interface IWriteRepository<T, TId>
    {
        void SaveOne(T entity);
        void SaveMany(IEnumerable<T> entities);
        void RemoveOne(T entity);
        void RemoveOne(TId id);
        void RemoveMany(IEnumerable<T> entities);
    }
}
