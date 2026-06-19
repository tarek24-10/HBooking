using HBook.Application.Common.Interfaces;
using HBook.Application.Common.Utility;
using HBook.Application.Services.Interface;
using HBook.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Services.Impelementation
{
    public class HotelService:IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HotelService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public void CreateHotel(Hotel hotel)
        {
            if (hotel.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(hotel.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\Hotels");

                using var filestream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);

                hotel.Image.CopyTo(filestream);

                hotel.ImageUrl = @"\images\Hotels\" + fileName;

            }
            else
            {
                hotel.ImageUrl = "https://placehold.co/600x400";
            }
            _unitOfWork.Hotel.Add(hotel);
            _unitOfWork.Save();
        }

        public bool DeleteHotel(int id)
        {
            try
            {
                var objFromDb = _unitOfWork.Hotel.Get(h => h.Id == id);
                if (objFromDb is not null)
                {
                    if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    _unitOfWork.Hotel.Delete(objFromDb);
                    _unitOfWork.Save();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Hotel> GetAllHotels()
        {
            return _unitOfWork.Hotel.GetAll(includeproperties: "Amenities");
        }

        public IEnumerable<Hotel> GetAvailableHotelsByDate(int nights, DateOnly checkInDate)
        {
            var hotelList = _unitOfWork.Hotel.GetAll(includeproperties: "Amenities");
            var roomList = _unitOfWork.Room.GetAll(includeproperties: "Hotel").ToList();
            var bookings = _unitOfWork.Booking.GetAll(b => b.Status == SD.StatusApproved
            || b.Status == SD.StatusCheckedIn).ToList();

            foreach (var hotel in hotelList)
            {
                int roomsAvailable = SD.HotelRoomsAvailable_Count(hotel.Id, roomList, bookings, checkInDate, nights);
                if (roomsAvailable == 0)
                {
                    hotel.IsAvailable = false;
                }
            }

            return hotelList;

        }

        public Hotel GetHotelById(int id)
        {
            return _unitOfWork.Hotel.Get(h => h.Id == id, includeproperties: "Amenities");
        }

        public void UpdateHotel(Hotel hotel)
        {
            if (hotel.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(hotel.Image.FileName);
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\Hotels");

                if (!string.IsNullOrEmpty(hotel.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\Hotels", hotel.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using var filestream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);

                hotel.Image.CopyTo(filestream);

                hotel.ImageUrl = @"\images\Hotels\" + fileName;

            }

            _unitOfWork.Hotel.Update(hotel);
            _unitOfWork.Save();
        }

        public bool IsAvailableByDate(int hotelId, int nights, DateOnly checkInDate)
        {
            var roomList = _unitOfWork.Room.GetAll().ToList();
            var bookings = _unitOfWork.Booking.GetAll(b => b.Status == SD.StatusApproved
            || b.Status == SD.StatusCheckedIn).ToList();


            int roomsAvailable = SD.HotelRoomsAvailable_Count(hotelId, roomList, bookings, checkInDate, nights);
            return roomsAvailable > 0;
        }
    }
}
