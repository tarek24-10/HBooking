using System.Linq.Expressions;

namespace HBook.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeproperties = null, bool tracked = false);
        public T Get(Expression<Func<T, bool>> filter, string? includeproperties = null, bool tracked = false);
        void Add(T entity);
        //void Update(T entity);
        void Delete(T entity);
        bool Any(Expression<Func<T, bool>> filter);
    }
}
