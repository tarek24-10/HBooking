using HBook.Domain.Entities;

namespace HBook.Application.Services.Interface
{
    public interface IHotelService
    {
        IEnumerable<Hotel> GetAllHotels();
        Hotel GetHotelById(int id);
        void CreateHotel(Hotel hotel);
        void UpdateHotel(Hotel hotel);
        bool DeleteHotel(int id);
        IEnumerable<Hotel> GetAvailableHotelsByDate(int nights, DateOnly checkInDate);
        bool IsAvailableByDate(int hotelId, int nights, DateOnly checkInDate);

    }
}
