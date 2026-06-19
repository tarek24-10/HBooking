using HBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Common.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        void Update(Booking entity);
    }
}
