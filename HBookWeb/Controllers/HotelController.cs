using HBook.Application.Common.Interfaces;
using HBook.Domain.Entities;
using HBook.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace HBookWeb.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelRepository hotelRepository;

        public HotelController(IHotelRepository hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        public IActionResult Index()
        {
            var hotels = hotelRepository.GetAll();
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
                hotelRepository.Add(hotel);
                hotelRepository.Save();
                TempData["success"] = "The hotel Has been created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Hotel could not be created";
            return View(hotel);
        }

        public IActionResult Update(int HotelId)
        {
            var hotel = hotelRepository.Get(h => h.Id == HotelId);
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
                hotelRepository.Update(hotel);
                hotelRepository.Save();
                TempData["success"] = "The hotel Has been updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Hotel could not be updated";
            return View(hotel);
        }


        public IActionResult Delete(int HotelId)
        {
            var hotel = hotelRepository.Get(h => h.Id == HotelId);
            if (hotel == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(hotel);
        }


        [HttpPost]
        public IActionResult Delete(Hotel hotel)
        {
            var objFromDb = hotelRepository.Get(h => h.Id == hotel.Id);
            if (objFromDb is not null)
            {
                hotelRepository.Delete(objFromDb);
                hotelRepository.Save();
                TempData["success"] = "The Hotel Has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Hotel could not be deleted";
            return View();
        }
    }
}
