using HBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Common.Interfaces
{
    public interface IRoomRepository : IRepository<Room>
    {
        public void Update(Room entity);
    }
}
