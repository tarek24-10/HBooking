using HBook.Domain.Entities;

namespace HBook.Application.Common.Interfaces
{
    public interface IAmenityRepository : IRepository<Amenity>
    {
        public void Update(Amenity entity);
    }
}
