using HBook.Application.Common.Interfaces;
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
    }
}
