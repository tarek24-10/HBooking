using HBook.Application.Common.Utility;
using HBook.Application.Services.Interface;
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
        private readonly IAmenityService _amenityService;
        private readonly IHotelService _hotelService;

        public AmenityController(IAmenityService amenityService, IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public IActionResult Index()
        {
            var amenities = _amenityService.GetAllAmenities();
            return View(amenities);
        }

        public IActionResult Create()
        {
            AmenityVM amenityVM = new AmenityVM()
            {
                HotelList = _hotelService.GetAllHotels().Select(h => new SelectListItem
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
                _amenityService.CreateAmenity(amenityVM.Amenity);
                TempData["success"] = "The Amenity Has been created successfully";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "The Amenity could not be created";
            }

            amenityVM.HotelList = _hotelService.GetAllHotels().Select(h => new SelectListItem
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
                Amenity = _amenityService.GetAmenityById(Id),
                HotelList = _hotelService.GetAllHotels().Select(h => new SelectListItem
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
                Amenity amenity = _amenityService.GetAmenityById(amenityVM.Amenity.Id);
                if (amenity.Name.ToLower().Contains("special"))
                {
                    amenityVM.Amenity.Description = amenity.Description;
                }
                else
                {
                    _amenityService.UpdateAmenity(amenityVM.Amenity);
                }
                TempData["success"] = "The Amenity Has been updated successfully";
                return RedirectToAction("Index");
            }


            TempData["error"] = "The Amenity could not be created";

            amenityVM.HotelList = _hotelService.GetAllHotels().Select(h => new SelectListItem
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
                Amenity = _amenityService.GetAmenityById(Id),
                HotelList = _hotelService.GetAllHotels().Select(h => new SelectListItem
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
            var objFromDB = _amenityService.GetAmenityById(amenityVM.Amenity.Id);
            if (objFromDB is not null)
            {
                var isdeleted = _amenityService.DeleteAmenity(objFromDB.Id);
                if (isdeleted)
                {
                    TempData["success"] = "The Amenity Has been deleted successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "The Amenity could not be deleted";
                }
            }
            return View(amenityVM);
        }
    }
}
