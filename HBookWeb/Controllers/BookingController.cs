using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBook.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HBookWeb.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public IActionResult FinalizeBooking(int HotelId, DateOnly CheckIn, int Nights)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            Booking booking = new Booking
            {
                UserId = userId,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                HotelId = HotelId,
                CheckInDate = CheckIn,
                Nights = Nights,
                Hotel = _unitOfWork.Hotel.Get(h => h.Id == HotelId, includeproperties: "Amenities"),
                CheckOutDate = CheckIn.AddDays(Nights),
            };
            booking.TotalCost = booking.Hotel.Price * Nights;

            return View(booking);
        }

        [HttpPost]
        public IActionResult FinalizeBooking(Booking booking)
        {
            var hotel = _unitOfWork.Hotel.Get(h => h.Id == booking.HotelId);
            booking.TotalCost = hotel.Price * booking.Nights;

            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;

            _unitOfWork.Booking.Add(booking);
            _unitOfWork.Save();
            return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
        }

        public IActionResult BookingConfirmation(int bookingId)
        {
            return View(bookingId);
        }
    }
}
