using HBook.Application.Common.Interfaces;
using HBook.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IHotelRepository Hotel { get; private set; }
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            Hotel = new HotelRepository(context);
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
