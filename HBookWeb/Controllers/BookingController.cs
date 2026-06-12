using HBook.Application.Common.Interfaces;
using HBook.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HBookWeb.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult FinalizeBooking(int HotelId, DateOnly CheckIn, int Nights)
        {
            Booking booking = new Booking
            {
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
