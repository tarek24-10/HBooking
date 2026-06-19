using HBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Services.Interface
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetAllBookings(string userId = "", string? status = "");
        Booking GetBookingById(int id);
        void CreateBooking(Booking booking);
    }
}
