using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBook.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System.Security.Claims;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

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
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult BookingDetails(int bookingId)
        {
            var booking = _unitOfWork.Booking.Get(b => b.Id == bookingId, includeproperties: "Hotel,User");
            booking.Hotel = _unitOfWork.Hotel.Get(h => h.Id == booking.Hotel.Id, includeproperties: "Amenities");

            if(booking.RoomNumber == 0 && booking.Status == SD.StatusApproved)
            {
                var availableRoomNumbers = GetAvailableRoomNumbersINHotel(booking.HotelId);

                booking.AvailableRooms = _unitOfWork.Room.GetAll(r => r.HotelId == booking.HotelId && availableRoomNumbers.Any(n => n == r.RoomNumber));
            }

            return View(booking);
        }

        [Authorize]
        public IActionResult FinalizeBooking(int HotelId, string CheckIn, int Nights)
        {
            var checkInDate = DateOnly.Parse(CheckIn);
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
                CheckInDate = checkInDate,
                Nights = Nights,
                Hotel = _unitOfWork.Hotel.Get(h => h.Id == HotelId, includeproperties: "Amenities"),
                CheckOutDate = checkInDate.AddDays(Nights),
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

            var domain = $"{Request.Scheme}://{Request.Host.Value}";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                SuccessUrl = domain + $"/Booking/BookingConfirmation?bookingId={booking.Id}",
                CancelUrl = domain + $"/Booking/FinalizeBooking?HotelId={booking.HotelId}&CheckIn={booking.CheckInDate}&Nights={booking.Nights}",
                Mode = "payment",
            };

            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(booking.TotalCost * 100), // Convert to cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = hotel.Name,
                        Description = $"Booking for {booking.Nights} nights at {hotel.Name}",
                    },
                },
                Quantity = 1,
            });

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.Booking.UpdateStripePaymentId(booking.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        public IActionResult BookingConfirmation(int bookingId)
        {
            var booking = _unitOfWork.Booking.Get(b => b.Id == bookingId, includeproperties: "Hotel,User");

            var service = new SessionService();
            Session session = service.Get(booking.StripeSessionId);

            if (booking.Status == SD.StatusPending)
            {
                _unitOfWork.Booking.UpdateBookingStatus(booking.Id, SD.StatusApproved, 0);
                _unitOfWork.Booking.UpdateStripePaymentId(booking.Id, booking.StripeSessionId, session.PaymentIntentId);
                _unitOfWork.Save();
            }


            return View(bookingId);
        }

        #region API Calls
        [HttpGet]
        [Authorize]
        public IActionResult GetAll(string status)
        {
            IEnumerable<Booking> bookings;
            if (!User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                bookings = _unitOfWork.Booking.GetAll(b => b.UserId == userId, includeproperties: "Hotel,User");
            }
            else
            {
                bookings = _unitOfWork.Booking.GetAll(includeproperties: "Hotel,User");
            }
            if (!string.IsNullOrEmpty(status))
            {
                bookings = bookings.Where(b => b.Status == status);
            }
            return Json(new { data = bookings });
        }

        #endregion

        private List<int> GetAvailableRoomNumbersINHotel(int hotelId)
        {
            List<int> availableRoomNumbers = new List<int>();

            var roomsInHotel = _unitOfWork.Room.GetAll(r => r.HotelId == hotelId);

            var checkedInRooms = _unitOfWork.Booking.GetAll(b => b.HotelId == hotelId && b.Status == SD.StatusCheckedIn).Select(b => b.RoomNumber);

            foreach (var room in roomsInHotel)
            {
                if (!checkedInRooms.Contains(room.RoomNumber))
                {
                    availableRoomNumbers.Add(room.RoomNumber);
                }
            }

            return availableRoomNumbers;
        }
    }
}
