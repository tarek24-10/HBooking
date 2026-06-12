using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBook.Domain.Entities;
using HBook.Infrastructure.Data;
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

        public void UpdateBookingStatus(int bookingId, string BookingStatus)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking != null)
            {
                booking.Status = BookingStatus;
                if(BookingStatus == SD.StatusCheckedIn)
                {
                    booking.ActualCheckInDate = DateTime.Now;
                }
                else if (BookingStatus == SD.StatusCompleted)
                {
                    booking.ActualCheckOutDate = DateTime.Now;
                }
                _context.Update(booking);
            }
        }

        public void UpdateStripePaymentId(int bookingId, string sessionId, string stripeIntentId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking != null)
            {
                booking.SessionId = sessionId;
                booking.StripePaymentIntentId = stripeIntentId;
                _context.Update(booking);
            }
        }
    }
}
