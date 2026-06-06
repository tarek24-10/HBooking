using HBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBook.Application.Common.Interfaces
{
    public interface IAmenityRepository : IRepository<Amenity>
    {
        public void Update(Amenity entity);
    }
}
