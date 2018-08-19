using System;
using System.Collections.Generic;
using System.Linq;

namespace SLib.DomainModel
{
    public interface IReadRepository<T, TId>
    {
        T GetById(TId id, Func<IQueryable<T>, IQueryable<T>> includesF = null, bool noTracking = false);
        IEnumerable<T> GetByIds(IEnumerable<TId> ids, Func<IQueryable<T>, IQueryable<T>> includesF = null, bool noTracking = false);
        IEnumerable<T> GetAll(Func<IQueryable<T>, IQueryable<T>> includesF = null, bool noTracking = false);
        bool Contains(T entity);
        bool Contains(TId id);
        bool IsAssociatedWithOtherExistingEntities(TId id);
        int Count();
        int Count(Func<T,bool> predicateF);
    }
}
