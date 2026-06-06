using HBook.Application.Common.Interfaces;
using HBook.Domain.Entities;
using HBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HBook.Infrastructure.Repository
{
    public class HotelRepository : IHotelRepository
    {
        private readonly ApplicationDbContext _context;
        public HotelRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Hotel entity)
        {
            _context.Add(entity);
        }

        public void Delete(Hotel entity)
        {
            _context.Remove(entity);
        }

        public Hotel Get(Expression<Func<Hotel, bool>> filter, string? includeproperties = null)
        {

            IQueryable<Hotel> query = _context.Set<Hotel>();

            if (filter is not null)
            {
                query.Where(filter);
            }

            // Hotel, Case sensetive
            if (includeproperties is not null)
            {
                foreach (var includeprop in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query.Include(includeprop);
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<Hotel> GetAll(Expression<Func<Hotel, bool>>? filter = null, string? includeproperties = null)
        {
            IQueryable<Hotel> query = _context.Set<Hotel>();

            if(filter is not null)
            {
                query.Where(filter);
            }

            if(includeproperties is not null)
            {
                foreach(var includeprop in includeproperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query.Include(includeprop);
                }
            }

            return query.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Hotel entity)
        {
            _context.Update(entity);
        }
    }
}
