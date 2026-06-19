using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBook.Domain.Entities;
using HBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Infrastructure.Repository
{
    internal class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;
         public BookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Booking entity)
        {
            _context.Update(entity);
        }

    }
}
