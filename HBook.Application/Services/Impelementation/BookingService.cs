using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBook.Application.Services.Interface;
using HBook.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Services.Impelementation
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateBooking(Booking booking)
        {
            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
        }

        public IEnumerable<Booking> GetAllBookings(string userId = "", string? statusFilterList = "")
        {
            IEnumerable<string> statusList = statusFilterList.ToLower().Split(",");
            if(!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(statusFilterList))
            {
                return _unitOfWork.Booking.GetAll(b => statusList.Contains(b.Status.ToLower()) 
                && b.UserId == userId, includeproperties: "Hotel,User");
            }
            else if (!string.IsNullOrEmpty(statusFilterList))
            {
                return _unitOfWork.Booking.GetAll(b => statusList.Contains(b.Status.ToLower())
                , includeproperties: "Hotel,User");
            }
            else if (!string.IsNullOrEmpty(userId))
            {
                return _unitOfWork.Booking.GetAll(b => b.UserId == userId
                , includeproperties: "Hotel,User");
            }
            else
            {
                return _unitOfWork.Booking.GetAll(includeproperties: "Hotel,User");
            }

        }

        public Booking GetBookingById(int id)
        {
            return _unitOfWork.Booking.Get(b => b.Id == id, includeproperties: "Hotel,User");
        }

        public IEnumerable<int> GetCheckedInRoomNumbers(int hotelId)
        {
            return _unitOfWork.Booking.GetAll(b => b.HotelId == hotelId && b.Status == SD.StatusCheckedIn).Select(b => b.RoomNumber);
        }

        public void UpdateBookingStatus(int bookingId, string BookingStatus, int roomNumber)
        {
            var booking = _unitOfWork.Booking.Get(b => b.Id == bookingId, tracked:true);
            if (booking != null)
            {
                booking.Status = BookingStatus;
                if (BookingStatus == SD.StatusCheckedIn)
                {
                    booking.RoomNumber = roomNumber;
                    booking.ActualCheckInDate = DateTime.Now;
                }
                else if (BookingStatus == SD.StatusCompleted)
                {
                    booking.ActualCheckOutDate = DateTime.Now;
                }
            }
            _unitOfWork.Save();
        }

        public void UpdateStripePaymentId(int bookingId, string sessionId, string stripeIntentId)
        {
            var booking = _unitOfWork.Booking.Get(b => b.Id == bookingId, tracked:true);
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
            _unitOfWork.Save();
        }
    }
}
