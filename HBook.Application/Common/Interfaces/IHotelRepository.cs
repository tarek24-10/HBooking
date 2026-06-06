using HBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HBook.Application.Common.Interfaces
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        //public IEnumerable<Hotel> GetAll(Expression<Func<Hotel, bool>>? filter = null, string? includeproperties = null);
        //public Hotel Get(Expression<Func<Hotel, bool>> filter, string? includeproperties = null);
        //void Add(Hotel entity);
        void Update(Hotel entity);
        //void Delete(Hotel entity);
    }
}
