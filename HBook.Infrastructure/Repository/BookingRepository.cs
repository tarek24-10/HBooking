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

        public void UpdateBookingStatus(int bookingId, string BookingStatus, int roomNumber)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking != null)
            {
                booking.Status = BookingStatus;
                if(BookingStatus == SD.StatusCheckedIn)
                {
                    booking.RoomNumber = roomNumber;
                    booking.ActualCheckInDate = DateTime.Now;
                }
                else if (BookingStatus == SD.StatusCompleted)
                {
                    booking.ActualCheckOutDate = DateTime.Now;
                }
            }
        }

        public void UpdateStripePaymentId(int bookingId, string sessionId, string stripeIntentId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (!string.IsNullOrEmpty(sessionId))
            {
                booking.StripeSessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(stripeIntentId))
            {
                booking.StripePaymentIntentId = stripeIntentId;
                booking.PaymentDate = DateTime.Now;
                booking.IsPaymentSuccessful = true;
            }
        }
    }
}
