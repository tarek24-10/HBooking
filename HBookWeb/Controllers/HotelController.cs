using HBook.Domain.Entities;
using HBook.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace HBookWeb.Controllers
{
    public class HotelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hotels = _context.Hotels.ToList();
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
                _context.Hotels.Add(hotel);
                _context.SaveChanges();
                TempData["success"] = "The hotel Has been created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Hotel could not be created";
            return View(hotel);
        }

        public IActionResult Update(int HotelId)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == HotelId);
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
                _context.Hotels.Update(hotel);
                _context.SaveChanges();
                TempData["success"] = "The hotel Has been updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Hotel could not be updated";
            return View(hotel);
        }


        public IActionResult Delete(int HotelId)
        {
            var hotel = _context.Hotels.FirstOrDefault(h => h.Id == HotelId);
            if (hotel == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(hotel);
        }


        [HttpPost]
        public IActionResult Delete(Hotel hotel)
        {
            var objFromDb = _context.Hotels.FirstOrDefault(h => h.Id == hotel.Id);
            if (objFromDb is not null)
            {
                _context.Hotels.Remove(objFromDb);
                _context.SaveChanges();
                TempData["success"] = "The Hotel Has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Hotel could not be deleted";
            return View();
        }
    }
}
