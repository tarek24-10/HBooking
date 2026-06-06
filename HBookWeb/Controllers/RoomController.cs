using HBook.Application.Common.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;

        public RoomController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var rooms = _unitOfWork.Room.GetAll(includeproperties: "Hotel");
            return View(rooms);
        }

        public IActionResult Create()
        {
            RoomVM roomVM = new RoomVM()
            {
                HotelList = _unitOfWork.Hotel.GetAll().Select(h => new SelectListItem
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
            bool isRoomNumderExist = _unitOfWork.Room.Any(r => r.RoomNumber == roomVM.Room.RoomNumber && r.HotelId == roomVM.Room.HotelId);
            //ModelState.Remove("Hotel");

            if (ModelState.IsValid && !isRoomNumderExist)
            {
                _unitOfWork.Room.Add(roomVM.Room);
                _unitOfWork.Save();
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

            roomVM.HotelList = _unitOfWork.Hotel.GetAll().Select(h => new SelectListItem
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
                Room = _unitOfWork.Room.Get(r => r.Id == Id),
                HotelList = _unitOfWork.Hotel.GetAll().Select(h => new SelectListItem
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
                _unitOfWork.Room.Update(roomVM.Room);
                _unitOfWork.Save();
                TempData["success"] = "The Room Has been updated successfully";
                return RedirectToAction("Index");
            }


            TempData["error"] = "The Room could not be created";

            roomVM.HotelList = _unitOfWork.Hotel.GetAll().Select(h => new SelectListItem
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
                Room = _unitOfWork.Room.Get(r => r.Id == Id),
                HotelList = _unitOfWork.Hotel.GetAll().Select(h => new SelectListItem
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
            var objFromDB = _unitOfWork.Room.Get(r => r.Id == roomVM.Room.Id);
            if (objFromDB is not null)
            {
                _unitOfWork.Room.Delete(objFromDB);
                _unitOfWork.Save();
                TempData["success"] = "The Room Has been deleted successfully";
                return RedirectToAction("Index");
            }


            TempData["error"] = "The Room could not be deleted";

            return View(roomVM);
        }
    }
}
