using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBookWeb.Models;
using HBookWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HBookWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                HotelList = _unitOfWork.Hotel.GetAll(includeproperties: "Amenities"),
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
                CheckOutDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Nights = 1
            };
            return View(homeVM);
        }

        [HttpPost]

        public IActionResult GetHotelByDate (int nights, DateOnly checkInDate)
        {
            //Thread.Sleep(250);
            var hotelList = _unitOfWork.Hotel.GetAll(includeproperties: "Amenities").ToList();
            var roomList = _unitOfWork.Room.GetAll().ToList();
            var bookingss = _unitOfWork.Booking.GetAll(b => b.Status == SD.StatusApproved 
            || b.Status == SD.StatusCheckedIn).ToList();

            foreach(var hotel in hotelList)
            {
                int roomsAvailable = SD.HotelRoomsAvailable_Count(hotel.Id, roomList, bookingss, checkInDate, nights);
                if (roomsAvailable == 0)
                {
                    hotel.IsAvailable = false;
                }
            }

            HomeVM homeVM = new HomeVM()
            {
                HotelList = hotelList,
                CheckInDate = checkInDate,
                Nights = nights
            };

            return PartialView("_HotelList" ,homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
