using HBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Services.Interface
{
    public interface IHotelService
    {
        IEnumerable<Hotel> GetAllHotels();
        Hotel GetHotelById(int id);
        void CreateHotel(Hotel hotel);
        void UpdateHotel(Hotel hotel);
        bool DeleteHotel(int id);
    }
}
