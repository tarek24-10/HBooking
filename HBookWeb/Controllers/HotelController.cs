using HBook.Application.Services.Interface;
using HBook.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HBookWeb.Controllers
{
    [Authorize]
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public IActionResult Index()
        {
            var hotels = _hotelService.GetAllHotels();
            return View(hotels);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Hotel hotel)
        {
            if (hotel.Name == hotel.Description)
            {
                ModelState.AddModelError("", "The description cannot exactly match the name");
            }

            if (ModelState.IsValid)
            {
                _hotelService.CreateHotel(hotel);
                TempData["success"] = "The hotel Has been created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Hotel could not be created";
            return View(hotel);
        }

        public IActionResult Update(int HotelId)
        {
            var hotel = _hotelService.GetHotelById(HotelId);
            if (hotel == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(hotel);
        }


        [HttpPost]
        public IActionResult Update(Hotel hotel)
        {
            if (hotel.Name == hotel.Description)
            {
                ModelState.AddModelError("", "The description cannot exactly match the name");
            }

            if (ModelState.IsValid & hotel.Id > 0)
            {
                _hotelService.UpdateHotel(hotel);
                TempData["success"] = "The hotel Has been updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Hotel could not be updated";
            return View(hotel);
        }


        public IActionResult Delete(int HotelId)
        {
            var hotel = _hotelService.GetHotelById(HotelId);
            if (hotel == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(hotel);
        }


        [HttpPost]
        public IActionResult Delete(Hotel hotel)
        {
            bool isdeleted = _hotelService.DeleteHotel(hotel.Id);
            if (isdeleted)
            {
                TempData["success"] = "The Hotel Has been deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "The Hotel could not be deleted";
            }
            return View();
        }
    }
}
