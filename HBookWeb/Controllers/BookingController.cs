using HBook.Application.Common.Interfaces;
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
    }
}
