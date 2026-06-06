using HBook.Domain.Entities;
using HBook.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
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
            var rooms = _context.Rooms.ToList();
            return View(rooms);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Room room)
        {
            //ModelState.Remove("Hotel");
            if (ModelState.IsValid)
            {
                _context.Rooms.Add(room);
                _context.SaveChanges();
                TempData["success"] = "The Room Has been created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Room could not be created";
            return View(room);
        }
    }
}
