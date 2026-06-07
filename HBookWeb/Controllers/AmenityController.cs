using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBook.Domain.Entities;
using HBookWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HBookWeb.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeproperties: "Hotel");
            return View(amenities);
        }

        public IActionResult Create()
        {
            AmenityVM amenityVM = new AmenityVM()
            {
                HotelList = _unitOfWork.Hotel.GetAll().Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                })
            };

            return View(amenityVM);
        }


        [HttpPost]
        public IActionResult Create(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity Has been created successfully";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "The Amenity could not be created";
            }

            amenityVM.HotelList = _unitOfWork.Hotel.GetAll().Select(h => new SelectListItem
            {
                Text = h.Name,
                Value = h.Id.ToString()
            });

            return View(amenityVM);
        }

        public IActionResult Update(int Id)
        {
            AmenityVM amenityVM = new()
            {
                Amenity = _unitOfWork.Amenity.Get(a => a.Id == Id),
                HotelList = _unitOfWork.Hotel.GetAll().Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                })
            };

            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }


        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity Has been updated successfully";
                return RedirectToAction("Index");
            }


            TempData["error"] = "The Amenity could not be created";

            amenityVM.HotelList = _unitOfWork.Hotel.GetAll().Select(h => new SelectListItem
            {
                Text = h.Name,
                Value = h.Id.ToString()
            });

            return View(amenityVM);
        }

        public IActionResult Delete(int Id)
        {
            AmenityVM amenityVM = new()
            {
                Amenity = _unitOfWork.Amenity.Get(a => a.Id == Id),
                HotelList = _unitOfWork.Hotel.GetAll().Select(h => new SelectListItem
                {
                    Text = h.Name,
                    Value = h.Id.ToString()
                })
            };

            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }


        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            var objFromDB = _unitOfWork.Amenity.Get(r => r.Id == amenityVM.Amenity.Id);
            if (objFromDB is not null)
            {
                _unitOfWork.Amenity.Delete(objFromDB);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity Has been deleted successfully";
                return RedirectToAction("Index");
            }


            TempData["error"] = "The Amenity could not be deleted";

            return View(amenityVM);
        }
    }
}
