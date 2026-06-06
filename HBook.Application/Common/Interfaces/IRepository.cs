using HBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HBook.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeproperties = null);
        public T Get(Expression<Func<T, bool>> filter, string? includeproperties = null);
        void Add(T entity);
        //void Update(T entity);
        void Delete(T entity);
        bool Any(Expression<Func<T, bool>> filter);
    }
}
