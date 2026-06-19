using HBook.Application.Common.Interfaces;
using HBook.Application.Services.Interface;
using HBook.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Services.Impelementation
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CheckRoomExists(int id)
        {
            return _unitOfWork.Room.Any(r => r.Id == id);
        }

        public void CreateRoom(Room room)
        {
            _unitOfWork.Room.Add(room);
            _unitOfWork.Save();
        }

        public bool DeleteRoom(int id)
        {
            try
            {
                var objFromDb = _unitOfWork.Room.Get(r => r.Id == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.Room.Delete(objFromDb);
                    _unitOfWork.Save();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _unitOfWork.Room.GetAll(includeproperties:"Hotel");
        }

        public Room GetRoomById(int id)
        {
            return _unitOfWork.Room.Get(r => r.Id == id, includeproperties: "Hotel");
        }

        public void UpdateRoom(Room room)
        {
            _unitOfWork.Room.Update(room);
            _unitOfWork.Save();
        }
    }
}
