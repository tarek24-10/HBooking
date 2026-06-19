using HBook.Application.Common.Interfaces;
using HBook.Application.Services.Interface;
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
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;

        public RoomController(IRoomService roomService, IHotelService hotelService)
        {
            _roomService = roomService;
            _hotelService = hotelService;
        }

        public IActionResult Index()
        {
            var rooms = _roomService.GetAllRooms();
            return View(rooms);
        }

        public IActionResult Create()
        {
            RoomVM roomVM = new RoomVM()
            {
                HotelList = _hotelService.GetAllHotels().Select(h => new SelectListItem
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
            bool isRoomNumderExist = _roomService.CheckRoomExists(roomVM.Room.Id);
            //ModelState.Remove("Hotel");

            if (ModelState.IsValid && !isRoomNumderExist)
            {
                _roomService.CreateRoom(roomVM.Room);
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

            roomVM.HotelList = _hotelService.GetAllHotels().Select(h => new SelectListItem
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
                Room = _roomService.GetRoomById(Id),
                HotelList = _hotelService.GetAllHotels().Select(h => new SelectListItem
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
                _roomService.UpdateRoom(roomVM.Room);
                TempData["success"] = "The Room Has been updated successfully";
                return RedirectToAction("Index");
            }


            TempData["error"] = "The Room could not be created";

            roomVM.HotelList = _hotelService.GetAllHotels().Select(h => new SelectListItem
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
                Room = _roomService.GetRoomById(Id),
                HotelList = _hotelService.GetAllHotels().Select(h => new SelectListItem
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
            var objFromDB = _roomService.GetRoomById(roomVM.Room.Id);
            if (objFromDB is not null)
            {
                _roomService.DeleteRoom(objFromDB.Id);
                TempData["success"] = "The Room Has been deleted successfully";
                return RedirectToAction("Index");
            }


            TempData["error"] = "The Room could not be deleted";

            return View(roomVM);
        }
    }
}
