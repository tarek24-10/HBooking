using HBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Services.Interface
{
    public interface IRoomService
    {
        IEnumerable<Room> GetAllRooms();
        Room GetRoomById(int id);
        void CreateRoom(Room room);
        void UpdateRoom(Room room);
        bool DeleteRoom(int id);
        bool CheckRoomExists(int id);
    }
}
