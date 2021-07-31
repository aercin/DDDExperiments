using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Common
{
    public interface IGenericRepository<T> where T : IAggregateRoot
    {
        IQueryable<T> All(bool isNoTracking = false);
        IQueryable<T> Find(Expression<Func<T, bool>> expression, bool isNoTracking = false);
        void Add(T entity);
        void Remove(T entity); 
    }
}
