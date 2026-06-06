using HBook.Application.Common.Interfaces;
using HBook.Domain.Entities;
using HBook.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Infrastructure.Repository
{
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext _context;
        public RoomRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Room entity)
        {
            _context.Update(entity);
        }
    }
}
