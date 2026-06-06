using HBook.Application.Common.Interfaces;
using HBook.Domain.Entities;
using HBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HBook.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeproperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            // Hotel, Case sensetive
            if (includeproperties is not null)
            {
                foreach (var includeprop in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop.Trim());
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeproperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            // Hotel, Case sensetive
            if (includeproperties is not null)
            {
                foreach (var includeprop in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop.Trim());
                }
            }

            return query.ToList();
        }
    }
}
