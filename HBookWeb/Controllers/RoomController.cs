using HBook.Domain.Entities;
using HBook.Infrastructure.Data;
using HBookWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HBookWeb.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RoomController(ApplicationDbContext context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var rooms = _context.Rooms.Include("Hotel").ToList();
            return View(rooms);
        }

        public IActionResult Create()
        {
            RoomVM roomVM = new RoomVM()
            {
                HotelList = _context.Hotels.Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                })
            };

            //IEnumerable<SelectListItem> hotelList = _context.Hotels.Select(h => new SelectListItem
            //{
            //    Text = h.Name,
            //    Value = h.Id.ToString()
            //});

            //ViewData["hotelList"] = hotelList;
            //ViewBag.HotelList = hotelList;
            return View(roomVM);
        }


        [HttpPost]
        public IActionResult Create(RoomVM roomVM)
        {
            bool isRoomNumderExist = _context.Rooms.Any(r => r.RoomNumber == roomVM.Room.RoomNumber && r.HotelId == roomVM.Room.HotelId);
            //ModelState.Remove("Hotel");

            if (ModelState.IsValid && !isRoomNumderExist)
            {
                _context.Rooms.Add(roomVM.Room);
                _context.SaveChanges();
                TempData["success"] = "The Room Has been created successfully";
                return RedirectToAction("Index");
            }

            if (isRoomNumderExist)
            {
                TempData["error"] = "The Room is already exist";
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "The Room could not be created";
            }

            roomVM.HotelList = _context.Hotels.Select(h => new SelectListItem
            {
                Text = h.Name,
                Value = h.Id.ToString()
            });

            return View(roomVM);
        }
    }
}
