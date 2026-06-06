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

        public IActionResult Update(int Id)
        {
            // EF Core Find requires key values in the same order as the entity key definition
            // The Room composite key is configured as (HotelId, RoomNumber) so pass keys in that order.
            RoomVM roomVM = new()
            {
                Room = _context.Rooms.Find(Id),
                HotelList = _context.Hotels.Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                })
            };

            if (roomVM.Room == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(roomVM);
        }


        [HttpPost]
        public IActionResult Update(RoomVM roomVM)
        {

            if (ModelState.IsValid)
            {
                _context.Rooms.Update(roomVM.Room);
                _context.SaveChanges();
                TempData["success"] = "The Room Has been updated successfully";
                return RedirectToAction("Index");
            }


            TempData["error"] = "The Room could not be created";

            roomVM.HotelList = _context.Hotels.Select(h => new SelectListItem
            {
                Text = h.Name,
                Value = h.Id.ToString()
            });

            return View(roomVM);
        }

        public IActionResult Delete(int Id)
        {
            RoomVM roomVM = new()
            {
                Room = _context.Rooms.Find(Id),
                HotelList = _context.Hotels.Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                })
            };

            if (roomVM.Room == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(roomVM);
        }


        [HttpPost]
        public IActionResult Delete(RoomVM roomVM)
        {
            var objFromDB = _context.Rooms.Find(roomVM.Room.Id);
            if (objFromDB is not null)
            {
                _context.Rooms.Remove(objFromDB);
                _context.SaveChanges();
                TempData["success"] = "The Room Has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }


            TempData["error"] = "The Room could not be deleted";

            return View();
        }
    }
}
