using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IHotelRepository Hotel { get; }
        IRoomRepository Room { get; }
        void Save();
    }
}
